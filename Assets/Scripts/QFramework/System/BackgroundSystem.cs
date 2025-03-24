using QFramework;
using UnityEngine;

namespace QFramework
{
    // 接口定义
    public interface IBackgroundSystem : ISystem
    {
        void StartMoving();
        void StopMoving();
    }
    
    // 实现类
    public class BackgroundSystem : AbstractSystem, IBackgroundSystem
    {
        private GameObject _backgroundsObj;
        private GameObject _landsObj;
        
        protected override void OnInit()
        {
            _backgroundsObj = GameObject.Find("Backgrounds");
            _landsObj = GameObject.Find("Lands");
        }
        
        public void StartMoving()
        {
            if (_backgroundsObj != null)
            {
                var controller = _backgroundsObj.GetComponent<BackgroundController>();
                if (controller != null)
                {
                    controller.isMove = true;
                }
            }
            
            if (_landsObj != null)
            {
                var controller = _landsObj.GetComponent<BackgroundController>();
                if (controller != null)
                {
                    controller.isMove = true;
                }
            }
        }
        
        public void StopMoving()
        {
            if (_backgroundsObj != null)
            {
                var controller = _backgroundsObj.GetComponent<BackgroundController>();
                if (controller != null)
                {
                    controller.isMove = false;
                }
            }
            
            if (_landsObj != null)
            {
                var controller = _landsObj.GetComponent<BackgroundController>();
                if (controller != null)
                {
                    controller.isMove = false;
                }
            }
        }
    }
}