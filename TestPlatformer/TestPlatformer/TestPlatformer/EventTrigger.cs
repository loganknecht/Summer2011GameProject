using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPlatformer
{
    class EventTrigger
    {
        bool isActivated;
        public bool isTriggerActivated {
            get {
                return isActivated;
            }
            set {
                isActivated = value;
            }
        }

        public EventTrigger() {
            isActivated = false;
        }
    }
}
