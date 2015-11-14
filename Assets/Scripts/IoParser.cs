using System.Collections;
using System.Collections.Generic;

public class IoParser {

	public int Count () {
		return ioData.Count;
	}

	public IoData Get () {
		if (Count () > 0) {
			IoData data = ioData[0];
			ioData.RemoveAt(0);
			return data;
		}
		return null;
	}

	List<IoData> ioData = new List<IoData> ();

	int state = 0;
	int getcount = 0;
	byte[] rbuf = new byte[16];

	public void Decode (byte inData) { 
		if (getcount < 16)
			rbuf[getcount] = inData;
		
		switch (state) {
		case 0:	// Find header
			if (inData == 0x02) {
				state++;
				getcount++;
			}
			break;
		case 1: // Save date
			getcount++;
			if (getcount >= 16) {
				if (rbuf[1] == 14 && Checksum(rbuf) == rbuf[14])
				{
					IoData tmpData = new IoData ();
					tmpData.from = rbuf[2];
					tmpData.to   = rbuf[3];
					tmpData.digital1 = rbuf[4];
					tmpData.digital2 = rbuf[5];
					for (int i=0; i<8; i++) {
						tmpData.analog[i] = rbuf[6+i];
					}
					ioData.Add (tmpData);
				}
				state = 0;
				getcount = 0;
			}
			break;
		default:
			state = 0;
			getcount = 0;
			break;
		}
	}

	/* checksum
 * 패킷 데이터의 무결성을 확인할 체크섬을 계산함
 * 체크섬은 패킷의 3번째 데이터부터 12개의 데이터를 모두 더한 값의 2의 보수로 함 
 */
	byte Checksum(byte[] buff) {
		byte sum = 0;
		for (int i=2; i<buff.Length-2; i++) {
			sum += buff[i];
		}
		return (byte)(0xff-sum+1);
	}
	

}
