using UnityEngine;
using System.Collections;

public abstract class CommunicationBase {
	public abstract bool Connect (string device, int baudrate = 9800);
	public abstract bool Disconnect ();
	public abstract bool IsConnected ();
	public abstract string[] GetDeviceList ();
	public abstract int Read (ref byte[] bytes, int len);
	public abstract int Write (byte[] bytes, int len);
	public abstract void Purge ();
}
