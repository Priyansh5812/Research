using GenericServiceLocator.Templates;
using UnityEngine;
using System.Collections.Generic;
using System;


namespace GenericServiceLocator
{

    namespace Templates
    {

        public interface IProvideService
        {
            public void PushService<T>(T service, bool canOverwrite) where T : class;
            public T PullService<T>(bool createIfNULL) where T : class, new();
            public void ClearService<T>(); 
            public void ClearAllServices();
            public bool PeekService<T>();

        }

        public interface IMonoService
        {
            public void RegisterService();
            public void UnregisterService();
        }

        public abstract class INonMonoService // This class will act as a notifier for Non MonoBehaviour Services
        {}

    }



    public sealed class ServiceLocator : IProvideService , IDisposable
    {
        private IProvideService i_Instance;
        private static ServiceLocator m_instance;
        public static ServiceLocator Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ServiceLocator();
                }
                return m_instance;
            }

        }

        

        private readonly Dictionary<System.Type, object> map;

        private ServiceLocator()
        {
            i_Instance = this;
            map = new Dictionary<System.Type, object>();
        }


        public bool PeekService<T>()
        {
            if (map.ContainsKey(typeof(T)))
            {
                return true;
            }
            return false;
        }

        public void ClearAllServices()
        {
            if (map != null)
            {
                map.Clear();
            }
            else
            {
                Debug.Log("Error Clearing Service Map! \n Check Initialization");
            }
        }

        public void ClearService<T>()
        {
            if (this.i_Instance.PeekService<T>())
            {
                map.Remove(typeof(T));
            }
            else
            {
                Debug.Log("Service does not exist in Map! \n Consider revising");
            }
        }

        public T PullService<T>(bool createIfNULL) where T : class , new()
        {
            if (this.i_Instance.PeekService<T>())
                return map[typeof(T)] as T;

            if (createIfNULL)
            {
                if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                    this.i_Instance.PushService<T>(CreateMonoInstance<T>(), false);

                else 
                    this.i_Instance.PushService<T>(CreateNonMonoInstance<T>(typeof(T)), false);
                
                return map[typeof(T)] as T;
            }
            Debug.Log("Service Does not exist for a Pull consider creating...");
            return null;
                
        }

        public void PushService<T>(T service, bool canOverwrite) where T : class
        {
            if (service == null)
            {
                Debug.Log("Null Service was intended to be registered!!!");
                return;
            }

            if (!this.i_Instance.PeekService<T>())
            {
                map[typeof(T)] = service;
                Debug.Log("Service of Type " + typeof(T) + " has been registered");
                return;
            }
            
            if (canOverwrite)
            {
                map[typeof(T)] = service;
                Debug.Log("Service of Type " + typeof(T) + "has been overridden");
                return;
            }

            //Debug.Log("Service already exists");

        }



        #region Service Creation Primitive

        private T CreateMonoInstance<T>()
        {
            GameObject obj = new GameObject(typeof(T).Name); // Gets Instantiated automatically
            obj.AddComponent(typeof(T));
            return obj.GetComponent<T>();
        }

        private T CreateNonMonoInstance<T>(Type t) where T : class , new()
        {   
            if (t.IsSubclassOf(typeof(INonMonoService)))
            {
                return new T();
            }
            Debug.LogError($"{typeof(T)} class does not implement INonMonoService interface\n the reference is returned NULL");
            return null;
        }
        #endregion
        public void Dispose()
        {
            this.i_Instance.ClearAllServices();
        }


    }


}
