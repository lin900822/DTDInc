using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool<T> where T : MonoBehaviour
{
    private Queue<T> effectPrefebs; //表示由effectPrefeb組成
    private GameObject effectPrefeb;
    private static EffectPool<T> _instance = null;

    //Singleton 單利模式
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
     * 另一種簡便的寫法
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

    //檢查還有多少特效可以用
    public int QueueCount
    {
        get{
            return effectPrefebs.Count;
        }
    }

    //產生特效
    public void InintPool(GameObject effectPrefeb)
    {
        this.effectPrefeb = effectPrefeb; //this指的是程式碼裡宣告的,後面的是傳進來的參數
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

    //回收特效
    public void Recycle(T obj)
    {
        effectPrefebs.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
 }
