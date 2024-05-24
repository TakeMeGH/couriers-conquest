using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
	public Camera _camera;
	public GameObject acacia;
	public GameObject beech;
	public GameObject birch;
	public GameObject dryTree;
	public GameObject fir;
	public GameObject juniper;
	public GameObject maple;
	public GameObject oak;
	public GameObject pine;
	public GameObject spruce;
	public GameObject sycamore;
	
	public void Acacia()
	{
		
		_camera.transform.position = new Vector3(77.6f, 30.5f, -84.4f);
		acacia.transform.rotation = Quaternion.identity;
		acacia.SetActive(true);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
		public void Beech()
	{
		_camera.transform.position = new Vector3(90.94f, 47.4f, -69.3f);
		beech.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(true);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
	
		public void Birch()
	{
		_camera.transform.position = new Vector3(90.94f, 49.6f, -89.6f);
		birch.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(true);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
	
		public void DryTree()
	{
		_camera.transform.position = new Vector3(90.94f, 28.7f, -35.2f);
		dryTree.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(true);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
		
		public void Fir()
	{
		_camera.transform.position = new Vector3(90.94f, 23.15f, -35.2f);
		fir.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(true);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
	
		public void Juniper()
	{
		_camera.transform.position = new Vector3(92.4f, 45.5f, -85f);
		juniper.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(true);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
	
		public void Maple()
	{
		_camera.transform.position = new Vector3(92.4f, 45.5f, -85f);
		maple.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(true);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
	
		public void Oak()
	{
		_camera.transform.position = new Vector3(90.4f, 45.9f, -100f);
		oak.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(true);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
	
		public void Pine()
	{
		_camera.transform.position = new Vector3(90.4f, 45.9f, -100f);
		pine.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(true);
		spruce.SetActive(false);
		sycamore.SetActive(false);
	}
	
		public void Spruce()
	{
		_camera.transform.position = new Vector3(100.0f, 61.8f, -120.8f);
		spruce.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(true);
		sycamore.SetActive(false);
	}
	
		public void Sycamore()
	{
		_camera.transform.position = new Vector3(90.4f, 61.8f, -80.1f);
		sycamore.transform.rotation = Quaternion.identity;
		acacia.SetActive(false);
		beech.SetActive(false);
		birch.SetActive(false);
		dryTree.SetActive(false);
		fir.SetActive(false);
		juniper.SetActive(false);
		maple.SetActive(false);
		oak.SetActive(false);
		pine.SetActive(false);
		spruce.SetActive(false);
		sycamore.SetActive(true);
	}
	
}
