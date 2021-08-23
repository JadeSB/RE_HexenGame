using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem.Views
{
    class CardView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public string Card { get; set; } = "PushBack";

        private Transform _parentToReturnTo = null;
        public void OnBeginDrag(PointerEventData eventData)
        {
            _parentToReturnTo = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            GameLoop.Instance.Select(Card);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(_parentToReturnTo);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void RemoveCard()
        {
            Destroy(this.gameObject);
        }
    }
}
