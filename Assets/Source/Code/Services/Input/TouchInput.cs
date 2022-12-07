using System;
using RedRockStudio.SZD.Services.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RedRockStudio.SZD
{
    public class TouchInput : MonoBehaviour, 
	    IPointerInput, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
	    public event Action<Vector2> Aimed;

	    public event Action<bool> ShootingChanged;

	    private bool _blocked;

	    public void OnPointerEnter(PointerEventData eventData) =>
		    _blocked = false;

	    public void OnPointerDown(PointerEventData eventData)
	    {
		    ShootingChanged?.Invoke(true);
		    Aimed?.Invoke(eventData.position);
	    }

	    public void OnDrag(PointerEventData eventData)
	    {
		    if(!_blocked)
			    Aimed?.Invoke(eventData.position);
	    }

	    public void OnPointerUp(PointerEventData eventData) =>
		    ShootingChanged?.Invoke(false);

	    public void OnPointerExit(PointerEventData eventData)
	    {
		    ShootingChanged?.Invoke(false);
		    _blocked = true;
	    }
    }
}
