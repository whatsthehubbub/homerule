using UnityEngine;
using System.Collections;

public class AgentUI : MonoBehaviour
{
	public void Close()
	{
		Destroy(this.gameObject);
		//en wat er code-wise moet gebeuren qua agent verplaatsen en alles
	}
}
