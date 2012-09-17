using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {
	
	public Color selectedColor;
	protected Renderer selectedRenderer;
	protected bool selectorEnable = true;
	protected Vector3 mousePosition;
	
	public virtual void Start(){
		selectedRenderer = this.renderer; 
	}
	
	public void HideMenu(){
		selectorEnable = false;
	}
	
	public void ShowMenu(){
		selectorEnable = true;
	}
	
	void OnLevelWasLoaded(int level) {
        HideMenu();
    }
	
	protected void NotifyHide(){
		foreach(GameObject selector in GameObject.FindGameObjectsWithTag("Selector")){
			selector.GetComponent<Selector>().HideMenu();
		}
		selectorEnable = true;
	}
	
	protected void NotifyShow(){
		foreach(GameObject selector in GameObject.FindGameObjectsWithTag("Selector")){
			selector.GetComponent<Selector>().ShowMenu();
		}
	}
	
	protected virtual void OnMouseDown(){
		NotifyHide();
		mousePosition = Input.mousePosition;
	}
	
	void OnMouseEnter(){
		PaintAll(selectedColor);
	}
	
	void OnMouseExit(){
		PaintAll(Color.white);
	}
	
	void Paint(Renderer r, Color c){
		foreach(Material m in r.materials)
			m.color = c;
	}
	
	void PaintAll(Color c){
		if(selectedRenderer == null) return;
		
		Paint(selectedRenderer, c);
		foreach(Renderer r in selectedRenderer.gameObject.GetComponentsInChildren<Renderer>()){
			Paint(r, c);
		}
	}

}
