using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.KCC;

[OrderBefore(typeof(PlayerController))]
[RequireComponent(typeof(PlayerController))]
public class PlayerInputHandler : NetworkBehaviour, IBeforeUpdate, IBeforeTick
{
	// PUBLIC MEMBERS

	[Networked]
	public NetworkBool InputBlocked { get; set; }

	/// <summary>
	/// Holds input for fixed update.
	/// </summary>
	public GameplayInput FixedInput => _fixedInput;

	/// <summary>
	/// Holds input for current frame render update.
	/// </summary>
	public GameplayInput RenderInput => _renderInput;

	/// <summary>
	/// Holds combined inputs from all render frames since last fixed update. Used when Fusion input poll is triggered.
	/// </summary>
	public GameplayInput CachedInput => _cachedInput;

	// PRIVATE MEMBERS

	// We need to store last known input to compare current input against (to track actions activation/deactivation). It is also used if an input for current frame is lost.
	// This is not needed on proxies, only input authority is registered to nameof(PlayerInput) interest group.
	[Networked(nameof(PlayerInputHandler))]
	private GameplayInput _lastKnownInput { get; set; }

	private GameplayInput _fixedInput;
	private GameplayInput _renderInput;
	private GameplayInput _cachedInput;
	private GameplayInput _baseFixedInput;
	private GameplayInput _baseRenderInput;

	private Vector2 _cachedMoveDirection;
	private float _cachedMoveDirectionSize;
	private bool _resetCachedInput;

	[SerializeField] private float mouseSensitivity = 10f;
	[SerializeField] private float mouseScrollSensitivity = 10f;

	// PUBLIC METHODS

	/// <summary>
	/// Check if the button is set in current input. FUN/Render input is resolved automatically.
	/// </summary>
	public bool IsSet(InputButtons button)
	{
		return Runner.Stage != default ? _fixedInput.Buttons.IsSet(button) : _renderInput.Buttons.IsSet(button);
	}

	/// <summary>
	/// Check if the button was pressed in current input.
	/// In FUN this method compares current fixed input agains previous fixed input.
	/// In Render this method compares current render input against previous render input OR current fixed input (first Render call after FUN).
	/// </summary>
	public bool WasPressed(InputButtons button)
	{
		return Runner.Stage != default ? _fixedInput.Buttons.WasPressed(_baseFixedInput.Buttons, button) : _renderInput.Buttons.WasPressed(_baseRenderInput.Buttons, button);
	}

	/// <summary>
	/// Check if the button was released in current input.
	/// In FUN this method compares current fixed input agains previous fixed input.
	/// In Render this method compares current render input against previous render input OR current fixed input (first Render call after FUN).
	/// </summary>
	public bool WasReleased(InputButtons button)
	{
		return Runner.Stage != default ? _fixedInput.Buttons.WasReleased(_baseFixedInput.Buttons, button) : _renderInput.Buttons.WasReleased(_baseRenderInput.Buttons, button);
	}

	public NetworkButtons GetPressedButtons()
	{
		return Runner.Stage != default ? _fixedInput.Buttons.GetPressed(_baseFixedInput.Buttons) : _renderInput.Buttons.GetPressed(_baseRenderInput.Buttons);
	}

	public NetworkButtons GetReleasedButtons()
	{
		return Runner.Stage != default ? _fixedInput.Buttons.GetReleased(_baseFixedInput.Buttons) : _renderInput.Buttons.GetReleased(_baseRenderInput.Buttons);
	}

	// NetworkBehaviour INTERFACE

	public override void Spawned()
	{
		// Reset to default state.
		_fixedInput = default;
		_renderInput = default;
		_cachedInput = default;
		_lastKnownInput = default;
		_baseFixedInput = default;
		_baseRenderInput = default;

		if (Runner.LocalPlayer == Object.InputAuthority)
		{
			var events = Runner.GetComponent<NetworkEvents>();
			events.OnInput.RemoveListener(OnInput);
			events.OnInput.AddListener(OnInput);

		}
	}

	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		var events = Runner.GetComponent<NetworkEvents>();
		events.OnInput.RemoveListener(OnInput);
	}

	// MONOBEHAVIOUR

	protected void Awake()
	{
		// Get PlayerController if needed
	}

	// IBeforeUpdate INTERFACE

	/// <summary>
	/// 1. Collect input from devices, can be executed multiple times between FixedUpdateNetwork() calls because of faster rendering speed.
	/// </summary>
	void IBeforeUpdate.BeforeUpdate()
	{
		if (Object == null || Object.HasInputAuthority == false)
			return;

		// Store last render input as a base to current render input.
		_baseRenderInput = _renderInput;

		// Reset input for current frame to default
		_renderInput = default;

		// Cached input was polled and explicit reset requested
		if (_resetCachedInput == true)
		{
			_resetCachedInput = false;

			_cachedMoveDirection = default;
			_cachedMoveDirectionSize = default;
			_cachedInput = default;
		}

		// Input is tracked only if the cursor is locked and runner should provide input
		if (Runner.ProvideInput == false || InputBlocked == true)
			return;

		ProcessStandaloneInput();
	}

	// IBeforeTick INTERFACE

	/// <summary>
	/// 3. Read input from Fusion. On input authority the FixedInput will match CachedInput.
	/// We have to prepare fixed input before tick so it is ready when read from other objects (agents)
	/// </summary>
	void IBeforeTick.BeforeTick()
	{
		if (InputBlocked == true)
		{
			_fixedInput = default;
			_baseFixedInput = default;
			_lastKnownInput = default;
			return;
		}

		// Store last known fixed input. This will be compared against new fixed input
		_baseFixedInput = _lastKnownInput;

		// Set correct fixed input (in case of resimulation, there will be value from the future)
		_fixedInput = _lastKnownInput;

		if (GetInput<GameplayInput>(out var input) == true)
		{
			_fixedInput = input;

			// Update last known input. Will be used next tick as base and fallback
			_lastKnownInput = input;
		}
		else
		{
			// In case we do not get input, clear look rotation delta so player will not rotate but repeat other actions
			_fixedInput.LookRotationDelta = default;
		}

		// The current fixed input will be used as a base to first Render after FUN
		_baseRenderInput = _fixedInput;
	}

	// PRIVATE METHODS

	/// <summary>
	/// 2. Push cached input and reset properties, can be executed multiple times within single Unity frame if the rendering speed is slower than Fusion simulation (or there is a performance spike).
	/// </summary>
	private void OnInput(NetworkRunner runner, NetworkInput networkInput)
	{
		if (InputBlocked == true)
			return;

		GameplayInput gameplayInput = _cachedInput;

		// Input is polled for single fixed update, but at this time we don't know how many times in a row OnInput() will be executed.
		// This is the reason for having a reset flag instead of resetting input immediately, otherwise we could lose input for next fixed updates (for example move direction).

		_resetCachedInput = true;

		// Now we reset all properties which should not propagate into next OnInput() call (for example LookRotationDelta - this must be applied only once and reset immediately).
		// If there's a spike, OnInput() and OnFixedUpdate() will be called multiple times in a row without OnBeforeUpdate() in between, so we don't reset move direction to preserve movement.
		// Instead, move direction and other sensitive properties are reset in next OnBeforeUpdate() - driven by _resetCachedInput.

		_cachedInput.LookRotationDelta = default;

		_cachedInput.MouseWheelDelta = default;

		// Input consumed by OnInput() call will be read in OnFixedUpdate() and immediately propagated to KCC.
		// Here we should reset render properties so they are not applied twice (fixed + render update).

		_renderInput.LookRotationDelta = default;

		_renderInput.MouseWheelDelta = default;

		networkInput.Set(gameplayInput);
	}

	private void ProcessStandaloneInput()
	{
		Vector2 moveDirection = Vector2.zero;
		Vector2 lookRotationDelta = new Vector2(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X")) * mouseSensitivity;

		if (Input.GetKey(KeyCode.W) == true) { moveDirection += Vector2.up; }
		if (Input.GetKey(KeyCode.S) == true) { moveDirection += Vector2.down; }
		if (Input.GetKey(KeyCode.A) == true) { moveDirection += Vector2.left; }
		if (Input.GetKey(KeyCode.D) == true) { moveDirection += Vector2.right; }

		if (moveDirection.IsZero() == false)
		{
			moveDirection.Normalize();
		}

		// Process input for render, represents current device state.

		_renderInput.MoveDirection = moveDirection * Time.deltaTime;
		_renderInput.LookRotationDelta = lookRotationDelta;

		_renderInput.MouseWheelDelta = -Input.mouseScrollDelta.y * mouseScrollSensitivity;

		_renderInput.Buttons.Set(InputButtons.Fire, Input.GetMouseButton(0));
		_renderInput.Buttons.Set(InputButtons.UseAbility, Input.GetMouseButton(1));
		_renderInput.Buttons.Set(InputButtons.Jump, Input.GetKey(KeyCode.Space));
		_renderInput.Buttons.Set(InputButtons.Run, Input.GetKey(KeyCode.LeftShift));

		// Process cached input for next OnInput() call, represents accumulated inputs for all render frames since last fixed update.

		float deltaTime = Time.deltaTime;

		// Move direction accumulation is a special case. Let's say simulation runs 30Hz (33.333ms delta time) and render runs 300Hz (3.333ms delta time).
		// If the player hits W key in last frame before fixed update, the KCC will move in render update by (velocity * 0.003333f).
		// Treating this input the same way for next fixed update results in KCC moving by (velocity * 0.03333f) - 10x more.
		// Following accumulation proportionally scales move direction so it reflects frames in which input was active.
		// This way the next fixed update will correspond more accurately to what happened in render frames.

		_cachedMoveDirection += moveDirection * deltaTime;
		_cachedMoveDirectionSize += deltaTime;

		_cachedInput.MoveDirection = _cachedMoveDirection / _cachedMoveDirectionSize;
		_cachedInput.LookRotationDelta += _renderInput.LookRotationDelta;
		_cachedInput.MouseWheelDelta += _renderInput.MouseWheelDelta;
		_cachedInput.Buttons = new NetworkButtons(_cachedInput.Buttons.Bits | _renderInput.Buttons.Bits);
	}
}
