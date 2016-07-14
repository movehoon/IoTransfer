using UnityEngine;
using System.Collections;

public class CommunicationManager : MonoBehaviour {

	CommunicationBase _comm;

	// Use this for initialization
	void Awake () {
#if UNITY_ANDROID
		_comm = new AndroidCommunication ();
#elif UNITY_STANDALONE
		_comm = new SerialCommunication ();
#else
		throw new System.NotImplementedException("Not implemented except android and windows");
#endif

//#if !UNITY_EDITOR
//		string [] devices = _comm.GetDeviceList ();
//		Debug.Log ("[CommunicationManager::Start] device count is " + devices.Length);
//		foreach (string device in devices) {
//			Debug.Log (device);
//		}
//#endif
	}

	void Start () {
//		Invoke ("AutoConnect", 1f);
	}

	void OnDestroy () {
		Disconnect ();
	}

	void AutoConnect () {
//		if (ConfigManager.SelectedDevice.Length > 0) 
//			Connect (ConfigManager.SelectedDevice);
	}

	public bool Connect (string device, int baudrate = 9800)
	{
		Debug.Log ("[CommunicationManager:Connect] Connect to device: " + device);
		if (IsConnected ())
			Disconnect ();
		return _comm.Connect (device, baudrate);
	}

	public bool Disconnect () 
	{
		return _comm.Disconnect ();
	}

	public bool IsConnected ()
	{
		return _comm.IsConnected ();
	}

	public string[] GetDeviceList () {
		return _comm.GetDeviceList ();
	}

	public int Write(byte[] buff) {
		return _comm.Write (buff, buff.Length);
	}

	public byte[] Read () {
		byte[] buff = new byte[2048];
		if (IsConnected ()) {
			int n = _comm.Read (ref buff, 2048);
			if (n > 0) {
				byte[] data = new byte[n];
				System.Buffer.BlockCopy (buff, 0, data, 0, n);
				return data;
			}
		}
//		Debug.Log ("[CommunicationManager:Read] nRead " + nRead.ToString ());
		return null;
	}

	public void Purge () {
		_comm.Purge ();
	}

	private static CommunicationManager _instance = null;
	public static CommunicationManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(CommunicationManager)) as CommunicationManager;
				if (_instance == null)
					Debug.LogError("There needs to be one active BehaviorManager script on a GameObject in your scene.");
				
			}
			return _instance;
		}
	}
}
