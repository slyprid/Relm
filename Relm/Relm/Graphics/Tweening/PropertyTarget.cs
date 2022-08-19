using System;
using System.Reflection;
using Relm.Graphics.Tweening.Interfaces;

namespace Relm.Graphics.Tweening
{
	class PropertyTarget<T> : ITweenTarget<T> where T : struct
    {
        protected object _target;
        FieldInfo _fieldInfo;
        protected Action<T> _setter;
        protected Func<T> _getter;


        public PropertyTarget(object target, string propertyName)
        {
            _target = target;

            // try to fetch the field. if we dont find it this is a property
            if ((_fieldInfo = Utils.GetFieldInfo(target, propertyName)) == null)
            {
                _setter = Utils.SetterForProperty<Action<T>>(target, propertyName);
                _getter = Utils.GetterForProperty<Func<T>>(target, propertyName);
            }

            Assert.IsTrue(_setter != null || _fieldInfo != null,
                "either the property (" + propertyName + ") setter or getter could not be found on the object " +
                target);
        }


        public object GetTargetObject()
        {
            return _target;
        }


        public void SetTweenedValue(T value)
        {
            if (_fieldInfo != null)
                _fieldInfo.SetValue(_target, value);
            else
                _setter(value);
        }


        public T GetTweenedValue()
        {
            if (_fieldInfo != null)
                return (T)_fieldInfo.GetValue(_target);

            return _getter();
        }
    }
}