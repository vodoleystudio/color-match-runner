using Codice.Client.BaseCommands.Differences;
using Codice.CM.SEIDInfo;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HyperCasual.Runner.LevelDefinition.Movment;
using Random = UnityEngine.Random;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class representing a Spawnable object.
    /// If a GameObject tagged "Player" collides
    /// with this object, it will trigger a fail
    /// state with the GameManager.
    /// </summary>
    public class Gate : Entity
    {
        private const string k_PlayerTag = "Player";
        private const float HideDuration = 0.2f;

        private const float k_HalfRangeTimeModificator = 0.5f;
        private const float k_HalfRangePositionMadificator = 0.5f;
        private const float k_FullRangeTimeMadificator = 1f;
        private Vector3 defaultScale;

        public bool IsUsed { get; private set; }
        public float MixValue { get; set; }

        private Transform _parent;

        protected override void Awake()
        {
            defaultScale = transform.localScale;
            _parent = transform.parent;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(k_PlayerTag))
            {
                transform.DOScale(Vector3.zero, HideDuration);
                transform.SetParent(PlayerController.Instance.transform);
                var currentPlayerColor = PlayerController.Instance.GetColor();
                PlayerController.Instance.SetColor(Color.Lerp(BaseColor, currentPlayerColor, MixValue));
                AudioManager.Instance.PlayEffect(SoundID.CloudSound);
            }
        }

        public override void ResetData()
        {
            IsUsed = false;
            transform.localScale = defaultScale;
            transform.SetParent(_parent);
        }

        public void Movment(LevelDefinition level)
        {
            StartCoroutine(MoveForSideToSide(level));
        }

        private IEnumerator MoveForSideToSide(LevelDefinition level)
        {
            if (level.GatesMovment.Directions.Count == 0)
            {
                throw new Exception("The number of directions can't be zero");
            }

            var couranteDirectionIndex = Random.Range(0, level.GatesMovment.Directions.Count);
            Vector3 movmentDirection = level.GatesMovment.Directions[couranteDirectionIndex].MovmentOffset;

            yield return new WaitForSeconds(Random.Range(level.GatesMovment.MaxAndMinStartTimeRange.y, level.GatesMovment.MaxAndMinStartTimeRange.x));

            if (level.GatesMovment.IsTheGatesCentrade)
            {
                yield return Move(level.GatesMovment.Directions[couranteDirectionIndex].MovmentOffset * k_HalfRangePositionMadificator + transform.position, k_HalfRangeTimeModificator);
            }

            while (true)
            {
                yield return Move(-movmentDirection + transform.position, k_FullRangeTimeMadificator);
                yield return Move(movmentDirection + transform.position, k_FullRangeTimeMadificator);

                if (Random.Range(0, 101) < level.GatesMovment.ProbabilityToChabgeDirectionInProcent)
                {
                    yield return Move(-movmentDirection * k_HalfRangePositionMadificator + transform.position, k_HalfRangeTimeModificator);
                    movmentDirection = ChooseRandomItemWithOutChosingTheActive(level.GatesMovment.Directions, movmentDirection);
                    yield return Move(movmentDirection * k_HalfRangePositionMadificator + transform.position, k_HalfRangeTimeModificator);
                }
            }

            IEnumerator Move(Vector3 offset, float timeModificator)
            {
                transform.DOMove(offset, level.GatesMovment.Duration * timeModificator).SetEase(level.GatesMovment.Directions[couranteDirectionIndex].Ease);
                yield return new WaitForSeconds(level.GatesMovment.Duration * timeModificator + level.GatesMovment.WaitTime);
            }

            Vector3 ChooseRandomItemWithOutChosingTheActive(List<MovmentDirections> list, Vector3 activeItem)
            {
                int index = Random.Range(0, list.Count);

                while (true)
                {
                    if (list[index].MovmentOffset != activeItem)
                    {
                        return list[index].MovmentOffset;
                    }
                    index = Random.Range(0, list.Count);
                }
            }
        }
    }
}