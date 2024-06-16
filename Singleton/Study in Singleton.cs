namespace Singleton
{
	public class ScoreKeeper
	{
		static ScoreKeeper _instance;
		public static ScoreKeeper Instance => instance ?? = new ScoreKeeper();
		
		private ScoreKeeper()
		{}
	}
	
}

/*

Generic Singleton for Monobehaviours

*/

public class GenericMonoSingleTon<T> : MonoBehaviour where T : Component
{
	protected static T instance = null;
	
	public static T Instance
	{
		get
		{
			if(!TryFindInstance())
			{
				CreateInstance();
			}
			
			return instance;
		}
		
	}
	
	private bool TryFindInstance()
	{
		instance = FindGameObjectByType<T>();
		
		return instance != null;
	}
	
	private T CreateInstance()
	{
		instance = new GameObject(typeof(T).Name + "Auto-Generated");
		instance.AddComponent<T>();
	}
	
	protected virtual void Awake()
	{
		InitializeSingleton();
	}
	
	protected virtual void InitializeSingleton()
	{
		if(instance == null)
		{
			instance = this as T;
		}
		else
		{
			if(instance != this as T)
			{
				Destroy(this);
			}
		}
	}
	
	
	
}
