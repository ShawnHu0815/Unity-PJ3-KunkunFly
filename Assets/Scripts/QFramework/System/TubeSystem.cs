using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QFramework
{
    // 接口定义
    public interface ITubeSystem : ISystem
    {
        void SpawnTube();
        void StartTubeMovement();
        void StopTubeMovement();
        void StartTubeSpawning();
        void StopTubeSpawning();
        void RecycleTube(GameObject tube);
    }
    
    // 实现类
    public class TubeSystem : AbstractSystem, ITubeSystem
    {
        private List<GameObject> _activeTubes = new List<GameObject>(); // 当前活跃的管道列表
        private GameObject _tubePrefab;
        private bool _isMoving = false;
        private bool _isSpawning = false;
        private float _spawnInterval = 1.6f;
        private IObjectPoolUtility _objectPool; // 对象池引用
        private Transform _tubesParent; // 管道的父物体
        
        protected override async void OnInit()
        {
            // 获取对象池工具
            _objectPool = this.GetUtility<IObjectPoolUtility>();
            
            // 使用Addressables加载预制体
            _tubePrefab = await this.GetUtility<IResourcesUtility>()
                                 .LoadAssetAsync<GameObject>("Prefabs/GreenTube");
            
            // 获取或创建管道父物体
            var tubesObj = GameObject.Find("Tubes");
            if (tubesObj != null)
            {
                _tubesParent = tubesObj.transform;
                var controller = tubesObj.GetComponent<TubesController>();
                if (controller != null && _tubePrefab == null)
                {
                    _tubePrefab = controller.greenTubePrefab;
                }
            }
            
            // 如果成功加载到预制体，预热对象池
            if (_tubePrefab != null && _objectPool != null)
            {
                // 预热10个管道对象
                _objectPool.PreWarm(_tubePrefab, 10);
                
                // 记录日志
                Debug.Log("管道对象池已预热，预创建了10个管道对象");
            }
        }
        
        public void SpawnTube()
        {
            if (_tubePrefab != null && _objectPool != null)
            {
                // 从对象池获取管道对象
                GameObject tube = _objectPool.GetObject(_tubePrefab);
                
                // 设置初始位置
                tube.transform.position = new Vector3(3.65f, 0, 0);
                
                // 获取控制器并设置属性
                var tubeController = tube.GetComponent<TubeController>();
                if (tubeController != null)
                {
                    tubeController.RandomHeight();
                    tubeController.isMove = _isMoving;
                    
                    // 添加离屏检测和回收的逻辑
                    TubeRecycleHandler recycleHandler = tube.GetComponent<TubeRecycleHandler>();
                    if (recycleHandler == null)
                    {
                        recycleHandler = tube.AddComponent<TubeRecycleHandler>();
                    }
                    recycleHandler.Initialize(this);
                }
                
                // 设置父物体
                if (_tubesParent != null)
                {
                    tube.transform.SetParent(_tubesParent);
                }
                
                // 添加到活跃列表
                _activeTubes.Add(tube);
            }
        }
        
        public void RecycleTube(GameObject tube)
        {
            if (tube != null)
            {
                _activeTubes.Remove(tube);
                _objectPool.RecycleObject(tube);
            }
        }
        
        public void StartTubeMovement()
        {
            _isMoving = true;
            foreach (var tube in _activeTubes)
            {
                if (tube != null)
                {
                    tube.GetComponent<TubeController>().isMove = true;
                }
            }
        }
        
        public void StopTubeMovement()
        {
            _isMoving = false;
            foreach (var tube in _activeTubes)
            {
                if (tube != null)
                {
                    tube.GetComponent<TubeController>().isMove = false;
                }
            }
        }
        
        public void StartTubeSpawning()
        {
            if (!_isSpawning)
            {
                _isSpawning = true;
                MonoBehaviourRuntime.Instance.StartCoroutine(SpawnTubesCoroutine());
            }
        }
        
        public void StopTubeSpawning()
        {
            _isSpawning = false;
        }
        
        private IEnumerator SpawnTubesCoroutine()
        {
            while (_isSpawning)
            {
                yield return new WaitForSeconds(_spawnInterval);
                
                if (this.GetModel<IGameModel>().IsGameStart.Value && _isMoving)
                {
                    SpawnTube();
                }
            }
        }
    }
    
    // 管道回收处理组件
    public class TubeRecycleHandler : MonoBehaviour
    {
        private ITubeSystem _tubeSystem;
        private Camera _mainCamera;
        
        public void Initialize(ITubeSystem tubeSystem)
        {
            _tubeSystem = tubeSystem;
            _mainCamera = Camera.main;
        }
        
        private void FixedUpdate()
        {
            if (_mainCamera != null && IsOffScreen())
            {
                _tubeSystem.RecycleTube(gameObject);
            }
        }
        
        private bool IsOffScreen()
        {
            Vector3 screenPoint = _mainCamera.WorldToViewportPoint(transform.position);
            return screenPoint.x < -0.3f;
        }
    }
}