using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool<T> where T : MonoBehaviour
{
    private Queue<T> effectPrefebs; //��ܥ�effectPrefeb�զ�
    private GameObject effectPrefeb;
    private static EffectPool<T> _instance = null;

    //Singleton ��Q�Ҧ�
    public static EffectPool<T> instance                                
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EffectPool<T>();
            }
            return _instance;
        }
    }

    /*
     * �t�@��²�K���g�k
     * public static EffectPool instance
     * {
     *      if(instance==null)
     *      {
     *          instance=this;
     *      }
     *      else
     *      {
     *          return instance;
     *      }
     * }
     */

    //�ˬd�٦��h�֯S�ĥi�H��
    public int QueueCount
    {
        get{
            return effectPrefebs.Count;
        }
    }

    //���ͯS��
    public void InintPool(GameObject effectPrefeb)
    {
        this.effectPrefeb = effectPrefeb; //this�����O�{���X�̫ŧi��,�᭱���O�Ƕi�Ӫ��Ѽ�
        effectPrefebs = new Queue<T>();
    }
     
    public T Spwan(Vector3 position, Quaternion quaternion)
    {
        if (effectPrefeb==null)
        {
            Debug.LogError(typeof(T).ToString() + " prefab not set!");
            return default(T);
        }
        if (QueueCount<=0)
        {
            GameObject effect = Object.Instantiate(effectPrefeb, position, quaternion);
            T t = effect.GetComponent<T>();
            if (t==null)
            {
                Debug.LogError(typeof(T).ToString() + " not found in prefab!");
                return default(T);
            }
        }
        T obj = effectPrefebs.Dequeue();
        obj.gameObject.transform.position = position;
        obj.gameObject.transform.rotation = quaternion;
        obj.gameObject.SetActive(true);
        return obj;
    }

    //�^���S��
    public void Recycle(T obj)
    {
        effectPrefebs.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
 }
