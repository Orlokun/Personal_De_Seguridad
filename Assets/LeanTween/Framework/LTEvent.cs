namespace LeanTween.Framework
{
    /**
* Object that describes the event to an event listener
* @class LTEvent
* @constructor
* @param {object} data:object Data that has been passed from the dispatchEvent method
*/
    public class LTEvent {
        public int id;
        public object data;

        public LTEvent(int id, object data){
            this.id = id;
            this.data = data;
        }
    }
}