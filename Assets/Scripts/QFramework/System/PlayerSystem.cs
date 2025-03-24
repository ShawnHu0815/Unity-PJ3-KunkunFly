using QFramework;
using UnityEngine;

// 接口和实现在同一个文件中
namespace QFramework
{
    // 接口定义部分
    public interface IPlayerSystem : ISystem
    {
        void SetupPlayer(GameObject player);
        void Jump();
        void CreateBall();
        void SetPlayerState(bool isFly, bool isSimulated = false);
    }
    
    // 实现部分
    public class PlayerSystem : AbstractSystem, IPlayerSystem
    {
        private GameObject _player;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private Transform _firePosition;
        private GameObject _ballPrefab;
        private float _flyForce = 6f;
        private Transform _playerRendererTransform;
        
        protected override void OnInit()
        {
            // 初始化代码
        }
        
        public void SetupPlayer(GameObject player)
        {
            _player = player;
            _rigidbody2D = _player.GetComponent<Rigidbody2D>();
            _animator = _player.GetComponent<Animator>();
            
            var playerController = _player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                _firePosition = playerController.firePosition;
                _ballPrefab = playerController.ballPrefab;
                _flyForce = playerController.flyForce;
                _playerRendererTransform = playerController.playerRendererTransform;
            }
        }
        
        public void Jump()
        {
            if (_rigidbody2D != null)
            {
                _rigidbody2D.velocity = new Vector2(0, _flyForce);
            }
        }
        
        public void CreateBall()
        {
            if (_ballPrefab != null && _firePosition != null)
            {
                // 生成球的随机位置偏移
                Vector2 positionOffset = new Vector2(
                    Random.Range(-0.2f, 0.2f),
                    Random.Range(-0.2f, 0.2f)
                );
    
                // 计算最终生成位置和旋转
                Vector2 spawnPosition = _firePosition.position + (Vector3)positionOffset;
                Quaternion spawnRotation = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));
    
                // 实例化球
                GameObject ball = GameObject.Instantiate(_ballPrefab, spawnPosition, spawnRotation);
                ball.SetActive(true);
            }
        }
        
        public void SetPlayerState(bool isFly, bool isSimulated = false)
        {
            if (_animator != null)
            {
                _animator.SetInteger("state", isFly ? 2 : 1);
            }
            
            if (_rigidbody2D != null)
            {
                _rigidbody2D.simulated = isSimulated;
            }
        }
    }
}