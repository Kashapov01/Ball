using Configs;
using Interfaces;
using Zenject;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private IGameBallProvider _ball;

        private float _minDistanceToMove;
        
        private Vector3 _startPos;
        private Vector3 _endPos;

        [Inject]
        private void Construct(IGameBallProvider ball, GameConfig config)
        {
            _ball = ball;
            _minDistanceToMove = config.Physics.minDistanceToMove;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPos = eventData.pointerCurrentRaycast.worldPosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _endPos = eventData.pointerCurrentRaycast.worldPosition;
            
            if (Vector3.Distance(_startPos, _endPos) > _minDistanceToMove) 
            {
                _ball.Move((_endPos - _startPos).normalized);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _ball.Move(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _ball.Move( Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _ball.Move(Vector3.back);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _ball.Move(Vector3.forward);
            }
        }
    }
}