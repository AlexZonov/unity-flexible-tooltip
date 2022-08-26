using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace com.flexford.packages.tooltip
{
	public partial class FlexibleTooltipAnimator : MonoBehaviour
	{
		[SerializeField]
		private FlexibleTooltip _tooltip;

		[SerializeField]
		private CanvasGroup _canvasGroup;

		[SerializeField]
		private bool _needDisbleAtInit = true;

		[SerializeField]
		private float _showDuration;

		[SerializeField]
		private float _idleDuration;

		[SerializeField]
		private float _hideDuration;

		private bool _didDisbleAtInit;
		private TweenerCore<float, float, FloatOptions> _showTween;
		private TweenerCore<float, float, FloatOptions> _hideTween;

		public bool IsShowing => _showTween != null && _showTween.IsPlaying();
		public bool IsHidding => _hideTween != null && _hideTween.IsPlaying();

		private void Reset()
		{
			_tooltip = GetComponent<FlexibleTooltip>();
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		private void OnEnable()
		{
			if (_needDisbleAtInit && !_didDisbleAtInit && Application.isPlaying)
			{
				MarkDisableAtInitCompletion();
				HideForce();
			}
		}

		private void OnDisable()
		{
			if (_showTween != null && _showTween.IsPlaying())
			{
				_showTween.Complete(true);
			}

			if (_hideTween != null && _hideTween.IsPlaying())
			{
				_hideTween.Complete(true);
			}
		}

		private void OnDestroy()
		{
			_showTween?.Kill(false);
			_hideTween?.Kill(false);
		}

		public void ShowForce()
		{
			Show(0f);
			_showTween.Complete(true);
		}

		public void Show()
		{
			Show(_showDuration);
		}

		public void Show(float showDuration)
		{
			ShowImpl(showDuration);
		}

		public void HideForce()
		{
			Hide(0f);
			_hideTween.Complete(true);
		}

		public void Hide()
		{
			Hide(_hideDuration);
		}

		public void Hide(float hideDuration)
		{
			HideImpl(hideDuration);
		}

		public void ShowAndHide()
		{
			ShowAndHide(_showDuration, _idleDuration, _hideDuration);
		}

		public void ShowAndHide(float idleDuration)
		{
			ShowAndHide(_showDuration, idleDuration, _hideDuration);
		}

		public void ShowAndHide(float showDuration, float idleDuration, float hideDuration)
		{
			ShowImpl(showDuration, null, OnCompleteCallback);

			void OnCompleteCallback()
			{
				Tween tween = HideImpl(hideDuration);
				tween.Rewind();
				tween.SetDelay(idleDuration);
				tween.Play();
			}
		}

		private Tween ShowImpl(float showDuration, TweenCallback startAction = null, TweenCallback completeAction = null)
		{
			PrepareSomeAnimation();

			void OnStartCallback()
			{
				gameObject.SetActive(true);
				_tooltip.ForceUpdateView();
				startAction?.Invoke();
			}

			float startValue = gameObject.activeSelf ? _canvasGroup.alpha : 0f;
			Tween tween = GetShowTween().ChangeValues(startValue, 1f, showDuration)
			                            .OnPlay(OnStartCallback)
			                            .OnComplete(completeAction);
			tween.Rewind(false);
			tween.Play();
			return tween;
		}

		private Tween HideImpl(float hideDuration, TweenCallback startAction = null, TweenCallback completeAction = null)
		{
			PrepareSomeAnimation();

			void OnCompleteCallback()
			{
				_canvasGroup.alpha = 0f;
				gameObject.SetActive(false);
				completeAction?.Invoke();
			}

			Tween tween = GetHideTween().ChangeEndValue(0f, hideDuration, true)
			                            .OnPlay(startAction)
			                            .OnComplete(OnCompleteCallback);
			tween.Rewind(false);
			tween.Play();
			return tween;
		}

		private TweenerCore<float, float, FloatOptions> GetShowTween()
		{
			if (_showTween == null)
			{
				_showTween = _canvasGroup.DOFade(1f, _showDuration)
				                         .SetAutoKill(false)
				                         .Pause();
			}

			return _showTween.SetDelay(0f);
		}

		private TweenerCore<float, float, FloatOptions> GetHideTween()
		{
			if (_hideTween == null)
			{
				_hideTween = _canvasGroup.DOFade(0f, _hideDuration)
				                         .SetAutoKill(false)
				                         .Pause();
			}

			return _hideTween;
		}

		private void PrepareSomeAnimation()
		{
			MarkDisableAtInitCompletion();
			_hideTween?.Pause();
			_showTween?.Pause();
		}

		private void MarkDisableAtInitCompletion()
		{
			_didDisbleAtInit = true;
		}
	}
}