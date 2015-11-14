using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Program : MonoBehaviour {

	public Image[] Host_Digital1;
	public Image[] Host_Digital2;
	public InputField[] Host_Analog;
	
	public Image[] Guest_Digital1;
	public Image[] Guest_Digital2;
	public InputField[] Guest_Analog;

	public Dropdown ComPort;

	bool getPacket = false;

	IoParser ioParser = new IoParser ();

//	byte count = 0;
	public void ScanPort () {
		ComPort.options.Clear ();
		foreach (string device in CommunicationManager.Instance.GetDeviceList ())
			ComPort.options.Add (new Dropdown.OptionData (device));
		ComPort.value = -1;
	}

	public void Connect () {
		try {
			string port = ComPort.options [ComPort.value].text;
			CommunicationManager.Instance.Connect (port);
		}
		catch (Exception e) {
			e.ToString ();
		}
//		SetHostDigital1 (count);
//		count++;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine ("Process");
	}

	bool _run = false;
	IEnumerator Process () {
		_run = true;
		while (_run) {
			yield return new WaitForSeconds (0.001f);
			if (CommunicationManager.Instance.IsConnected ()) {
				byte[] rbuf = CommunicationManager.Instance.Read ();
				if (rbuf == null)
					continue;

				foreach (byte b in rbuf) {
					ioParser.Decode (b);
				}

				while (ioParser.Count () > 0) {
					IoData data = ioParser.Get ();
					switch (data.from) {
					case 0:
						SetHostDigital1 (data.digital1);
						SetHostDigital2 (data.digital2);
						for (int i=0; i<8; i++) {
							float value = (float)data.analog[i];
							Host_Analog[i].text = (value*5/255).ToString("0.00") + "v (" + ((int)value).ToString () + ")";
						}
						break;
					case 1:
						SetGuestDigital1 (data.digital1);
						SetGuestDigital2 (data.digital2);
						for (int i=0; i<8; i++) {
							float value = (float)data.analog[i];
							Guest_Analog[i].text = (value*5/255).ToString("0.00") + "v (" + ((int)value).ToString () + ")";
						}
						break;
					}
				}
			}
		}
		yield return null;
	}



	void SetHostDigital1(byte digital) {
		for (int i = 0; i < 8; i++) {
			Host_Digital1[i].gameObject.SetActive ((digital & (0x01<<i)) != 0);
		}
	}
	void SetHostDigital2(byte digital) {
		for (int i = 0; i < 8; i++) {
			Host_Digital2[i].gameObject.SetActive ((digital & (0x01<<i)) != 0);
		}
	}
	void SetGuestDigital1(byte digital) {
		for (int i = 0; i < 8; i++) {
			Guest_Digital1[i].gameObject.SetActive ((digital & (0x01<<i)) != 0);
		}
	}
	void SetGuestDigital2(byte digital) {
		for (int i = 0; i < 8; i++) {
			Guest_Digital2[i].gameObject.SetActive ((digital & (0x01<<i)) != 0);
		}
	}
}
