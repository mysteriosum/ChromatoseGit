using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("ManagerCollection")]
public class ManagerContainer : MonoBehaviour {

	[XmlArrayItem("Managers")]
	public List<ChromatoseManager> Managers = new List<ChromatoseManager>();
}
