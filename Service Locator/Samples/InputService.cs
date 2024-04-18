using UnityEngine;
using GenericServiceLocator;
using GenericServiceLocator.Templates;
using Service = GenericServiceLocator.Templates.IProvideService;
using System;


public class InputService : MonoBehaviour , IMonoService
{

    #region Axis Control
    public float HorizontalInput { get; private set;}
    public float VerticalInput { get; private set; }
    public float HorizontalInputRaw { get; private set;}
    public float VerticalInputRaw { get; private set; }



    public float HorizontalSens = 1f;

    public float VerticalSens = 1f;
    #endregion

    #region Actions
    public event Action OnVerticalInput;
    #endregion

    void Awake()
    {
        RegisterService();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal") * HorizontalSens;
        VerticalInput = Input.GetAxis("Vertical") * VerticalSens;
        HorizontalInputRaw = Input.GetAxisRaw("Horizontal");
        VerticalInputRaw = Input.GetAxisRaw("Vertical");
        InvokeListeners();
    }

    private void InvokeListeners()
    {
        if(Input.GetAxisRaw("Vertical") == 1f)
        {
            OnVerticalInput.Invoke();  
        }
    }



    private void OnDisable()
    {
       UnregisterService();
       ClearListener();
    }

    private void ClearListener()
    {
        Delegate[] arr = OnVerticalInput.GetInvocationList();
        foreach (Delegate a in arr) 
        {
            OnVerticalInput -= (Action)a;
        }

    }


    #region interface

    public void UnregisterService()
    {
        (ServiceLocator.Instance as IProvideService).ClearService<InputService>();
    }

    public void RegisterService()
    {
        (ServiceLocator.Instance as IProvideService).PushService<InputService>(this , true);
    }

    #endregion

}
