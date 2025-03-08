using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Menus
{
    public abstract class MenuAnimation : MonoBehaviour
    {
        protected enum SlideDirection { Left, Right, Top, Bottom }

        #region Variables
        [field: Header("Fade Animations")]
        [field: SerializeField] public bool fade { get; private set; }

        [field: Header("Grow Animations")]
        [field: SerializeField] public bool grow { get; private set; }

        [field: Header("Spin Animations")]
        [field: SerializeField] public bool spin { get; private set; }

        [Tooltip("The number of spins if using the Spin animation type")]
        [Range(0, 5)]
        [SerializeField]
        protected int numSpins;

        [field: Header("Slide Animations")]
        [field: SerializeField] public bool slide { get; private set; }

        [Tooltip("Direction the panel should slide in from, if using a slide animation. NOTE:" +
          "To work properly, the panel should have its AnchorMin and AnchorMax aligned to the side of" +
          "the screen it is sliding in from.")]
        [SerializeField] protected SlideDirection slideDirection;

        [Tooltip("This is the distance the slide will travel, in pixels, in the selected direction.")]
        [SerializeField] protected float slideDistance = 1000f;

        public bool fadeComplete { get; protected set; }
        public bool growComplete { get; protected set; }
        public bool spinComplete { get; protected set; }
        public bool slideComplete { get; protected set; }

        [Header("Animation Settings")]
        [SerializeField]
        protected GameObject animationPanel;

        [Range(0, 1)]
        [SerializeField]
        protected float appearSpeed = 0.5f;

        [SerializeField]
        protected Ease animationEase = Ease.Linear;

        protected RectTransform rectTransform;
        protected Vector2 pivotDefault;

        protected Vector2 initialScale;
        protected Vector2 initialPosition;
        protected CanvasGroup group;
        protected Menu attachedMenu;
        #endregion

        private void Awake()
        {
            rectTransform = animationPanel.GetComponent<RectTransform>();
            initialPosition = rectTransform.anchoredPosition;
            pivotDefault = rectTransform.pivot;
            initialScale = rectTransform.localScale;
            attachedMenu = GetComponent<Menu>();
        }

        protected virtual void OnEnable()
        {
            // Reset the animation panel
            rectTransform.anchoredPosition = initialPosition;
            rectTransform.localScale = initialScale;
            pivotDefault = rectTransform.pivot;
            initialScale = rectTransform.localScale;

            if (fade && group == null)
            {
                animationPanel.TryGetComponent<CanvasGroup>(out group);

                // If still no canvas group - add it instead
                if (!group) group = animationPanel.AddComponent<CanvasGroup>();
            }
        }

        protected void SlideVertical(float endPos)
        {
            rectTransform.DOAnchorPosY(endPos, appearSpeed).
                SetEase(animationEase).
                OnComplete(() =>
                {
                    attachedMenu.IsOpening = false;
                    slideComplete = true;
                });
        }

        protected void SlideHorizontal(float endPos)
        {
            rectTransform.DOAnchorPosX(endPos, appearSpeed).
                SetEase(animationEase).
                OnComplete(() =>
                {
                    attachedMenu.IsOpening = false;
                    slideComplete = true;
                });
        }
    }
}
