using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GUIManager : MonoBehaviour {
	public Text guiTextMode;
	public Slider sizeSlider;
	public TXTPaintTouchTest painter;

	public void SetBrushMode(int newMode){
		PainterTouch_BrushMode brushMode =newMode==0? PainterTouch_BrushMode.DECAL: PainterTouch_BrushMode.PAINT; //Cant set enums for buttons :(
		string colorText=brushMode== PainterTouch_BrushMode.PAINT?"orange":"purple";	
		guiTextMode.text="<b>Mode:</b><color="+colorText+">"+brushMode.ToString()+"</color>";
		painter.SetBrushMode (brushMode);
	}
	public void UpdateSizeSlider(){
		painter.SetBrushSize (sizeSlider.value);
	}
}
