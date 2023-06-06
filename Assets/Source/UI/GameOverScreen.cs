using System;
using System.Collections;
using UnityEngine;

namespace Assets.Source.UI
{
    public class GameOverScreen : Screen
    {
        public event Action Clicked;

        private void Start()
        {
            Hide();
        }

        public override void Show()
        {
            base.Show();
            StartCoroutine(WaitClick());
        }

        private IEnumerator WaitClick()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            Clicked?.Invoke();

            Hide();
        }
    }
}