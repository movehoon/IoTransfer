using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO.Ports;

public class SerialCommunication : CommunicationBase {
#if UNITY_STANDALONE
	public SerialPort serialPort = new SerialPort ();
#endif

	public override bool Connect (string device, int baudrate = 115200) {
		#if UNITY_STANDALONE
		try {
			serialPort.BaudRate = baudrate;
			serialPort.PortName = device;
			serialPort.ReadTimeout = 10;
			serialPort.WriteTimeout = 10;
			serialPort.Open ();
			if (serialPort.IsOpen)
				return true;
			else
				return false;
		} catch (Exception e) {
			Debug.Log (e.ToString ());
			return false;
		}
		#else
		throw new System.NotImplementedException("Not implemented except android and windows");
		#endif
	}
	
	public override bool Disconnect () {
		#if UNITY_STANDALONE
		serialPort.Close ();
		return false;
		#else
		throw new System.NotImplementedException("Not implemented except android and windows");
		#endif
	}
	
	public override bool IsConnected () {
		#if UNITY_STANDALONE
		return serialPort.IsOpen;
		#else
		throw new System.NotImplementedException("Not implemented except android and windows");
		#endif
	}

	public override string[] GetDeviceList () {
		#if UNITY_STANDALONE_WIN
		return System.IO.Ports.SerialPort.GetPortNames ();
		#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
		List<string> devices = new List<string> ();
		string[] files = System.IO.Directory.GetFiles("/dev/");
		foreach (var d in files) {
			if (d.StartsWith("/dev/tty.") || d.StartsWith("/dev/ttyUSB")) {
				devices.Add (d);
			}
		}
		return devices.ToArray ();
		#else
		throw new System.NotImplementedException("Not implemented except android and windows");
		#endif
	}

	public override int Read (ref byte[] bytes, int len) {
		#if UNITY_STANDALONE
		int result = -1;
		try {
			result = serialPort.Read (bytes, 0, len);
		}
		catch (Exception e) {
//			Debug.Log (e.ToString ());
		}
		return result;
		#else
		throw new System.NotImplementedException("Not implemented except android and windows");
		#endif
	}

	public override int Write (byte[] bytes, int len) {
		#if UNITY_STANDALONE
		try {
		serialPort.Write (bytes, 0, len);
		}
		catch (Exception e) {
			Debug.Log (e.ToString ());
		}
		return len;
		#else
		throw new System.NotImplementedException("Not implemented except android and windows");
		#endif
	}

	public override void Purge () {
	}
}
