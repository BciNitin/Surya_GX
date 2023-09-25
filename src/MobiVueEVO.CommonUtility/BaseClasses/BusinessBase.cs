using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace MobiVUE.Utility
{
    [DataContract]
    public class BusinessBase
    {
        private BusinessBase _stateObject;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        [DataMember]
        [Browsable(false)]
        public bool IsDeleted { get; private set; } = false;

        [Browsable(false)]
        public bool IsDirty
        {
            get { return IsSelfDirty || ForAnyProperties(x => x.IsDirty); }
        }

        [DataMember]
        [Browsable(false)]
        public bool IsNew { get; private set; } = true;

        [DataMember]
        [Browsable(false)]
        public bool IsSelfDirty { get; private set; } = false;

        #endregion Properties

        #region Functions

        public T DeepClone<T>()
            where T : BusinessBase, new()
        {
            if (!(this is T)) { throw new MobiVUEException("Invalid type."); }

            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            ser.WriteObject(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)ser.ReadObject(stream);
            stream.Close();

            UpdateStateToEachProperty(this, result);

            return result;
        }


        public bool IsValid(out List<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(this);
            List<ValidationResult> results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this, validationContext, results, true);
            BusinessRuleList ruleList = new BusinessRuleList();
            AddBusinessRules(ruleList);
            if (ruleList._businessRules.HaveItems())
            {
                foreach (var bizRule in ruleList._businessRules)
                {
                    if (!bizRule.Key.Invoke())
                    {
                        results.Add(new ValidationResult(bizRule.Value));
                        if (isValid) isValid = false;
                    }
                }
            }

            List<ValidationResult> loopResults = new List<ValidationResult>();
            IterateToProperties((x) =>
            {
                loopResults = new List<ValidationResult>();
                bool isChildValid = x.IsValid(out loopResults);
                //validationContext = new ValidationContext(x);
                //bool isChildValid = Validator.TryValidateObject(x, validationContext, loopResults, true);
                results.AddRange(loopResults);
                isValid = isValid && isChildValid;

            });
            validationResults = results;
            return isValid;
        }

        public void MarkAsDeleted()
        {
            this.IsSelfDirty = true;
            this.IsDeleted = true;
        }

        public void MarkAsNew()
        {
            IsNew = true;
        }

        public void MarkAsOld()
        {
            IsNew = false;
            IsSelfDirty = false;
            IsDeleted = false;
            IterateToProperties((s) => { s.IsNew = false; s.IsSelfDirty = false; s.IsDeleted = false; });
        }

        public void MarkDirty()
        {
            IsSelfDirty = true;
        }

        public void Validate()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            IsValid(out validationResults);
            if (validationResults.Count > 0)
            {
                throw new MobiVUEException(String.Join("\n", validationResults));
            }
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null) return;

            var property = (MemberExpression)propertyExpression.Body;
            VerifyPropertyExpression<T>(propertyExpression, property);
            this.OnPropertyChanged(property.Member.Name);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<TPropertyType>(ref TPropertyType refValue, TPropertyType newValue, Expression<Func<TPropertyType>> propertyExpression)
        {
            if (!object.Equals(refValue, newValue))
            {
                refValue = newValue;
                IsSelfDirty = true;
                this.OnPropertyChanged(propertyExpression);
            }
        }

        protected delegate bool ValidationDelegate();

        protected virtual void AddBusinessRules(BusinessRuleList rules)
        {
            //rules.AddRules(() => { return true; }, "");
        }

        private bool ForAnyProperties(Func<BusinessBase, bool> confition)
        {
            IList<PropertyInfo> props = new List<PropertyInfo>(this.GetType().GetProperties());
            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType.BaseType == typeof(BusinessBase))
                {
                    BusinessBase propValue = prop.GetValue(this) as BusinessBase;
                    if (propValue != null)
                    {
                        if (confition(propValue) == true || propValue.ForAnyProperties(confition))
                        {
                            return true;
                        }
                    }
                }
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GenericTypeArguments.Length == 1 && prop.PropertyType.GenericTypeArguments[0].IsSubclassOf(typeof(BusinessBase)))
                {
                    if (prop.PropertyType.GetInterface("IEnumerable") != null)
                    {
                        var collection = (IEnumerable)prop.GetValue(this);
                        if (collection != null)
                        {
                            foreach (var item in collection)
                            {
                                var bo = item as BusinessBase;
                                if (confition(bo) == true || (bo).ForAnyProperties(confition))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        BusinessBase propValue = prop.GetValue(this) as BusinessBase;
                        if (propValue != null)
                        {
                            if (confition(propValue) == true || propValue.ForAnyProperties(confition))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void IterateToProperties(Action<BusinessBase> action)
        {
            IList<PropertyInfo> props = new List<PropertyInfo>(this.GetType().GetProperties());
            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType.BaseType == typeof(BusinessBase))
                {
                    BusinessBase propValue = prop.GetValue(this) as BusinessBase;
                    if (propValue != null)
                    {
                        propValue.IterateToProperties(action);
                        action(propValue);
                    }
                }

                if (prop.PropertyType.IsGenericType && prop.PropertyType.GenericTypeArguments.Length == 1 && prop.PropertyType.GenericTypeArguments[0].IsSubclassOf(typeof(BusinessBase)))
                {
                    if (prop.PropertyType.GetInterface("IEnumerable") != null)
                    {
                        var collection = (IEnumerable)prop.GetValue(this);
                        if (collection != null)
                        {
                            foreach (var item in collection)
                            {
                                var bo = item as BusinessBase;
                                bo.IterateToProperties(action);
                                action(bo);
                            }
                        }
                    }
                    else
                    {
                        BusinessBase propValue = prop.GetValue(this) as BusinessBase;
                        if (propValue != null)
                        {
                            propValue.IterateToProperties(action);
                            action(propValue);
                        }
                    }
                }
            }
        }

        private void MarkAsSelfOld()
        {
            IsSelfDirty = false;
        }

        private object ObjectDeepClone()
        {
            DataContractSerializer ser = new DataContractSerializer(this.GetType());
            object result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                ser.WriteObject(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                result = ser.ReadObject(stream);
                stream.Close();
                UpdateStateToEachProperty(this, (BusinessBase)result);
            }
            return result;
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyExpression<T>(Expression<Func<T>> propertyExpresion, MemberExpression property)
        {
            if (property.Member.GetType().IsAssignableFrom(typeof(PropertyInfo)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid Property Expression {0}", propertyExpresion));
            }

            var instance = property.Expression as ConstantExpression;
            if (instance.Value != this)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid Property Expression {0}", propertyExpresion));
            }
        }

        public void AcceptEdit()
        {
            _stateObject = null;
        }

        public void BeginEdit()
        {
            _stateObject = (BusinessBase)this.ObjectDeepClone();
        }

        public void CancelEdit()
        {
            if (_stateObject == null) return;

            CopyValueToEachProperty(_stateObject);
            _stateObject = null;
        }

        public bool IsEditing()
        {
            return _stateObject != null;
        }

        private static void UpdateStateToEachProperty<T, R>(T original, R coppied)
            where T : BusinessBase where R : BusinessBase
        {
            if (!original.IsSelfDirty) coppied.MarkAsSelfOld();

            IList<PropertyInfo> props = new List<PropertyInfo>(coppied.GetType().GetProperties());

            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType.BaseType == typeof(BusinessBase))
                {
                    BusinessBase coppiedValue = prop.GetValue(coppied) as BusinessBase;
                    if (coppiedValue != null)
                    {
                        BusinessBase originalValue = prop.GetValue(original) as BusinessBase;
                        UpdateStateToEachProperty(originalValue, coppiedValue);
                        if (!originalValue.IsSelfDirty)
                            coppiedValue.MarkAsSelfOld();
                    }
                }
                else if (prop.PropertyType.IsGenericType && prop.PropertyType.GenericTypeArguments.Length == 1 && prop.PropertyType.GenericTypeArguments[0].IsSubclassOf(typeof(BusinessBase)))
                {
                    if (prop.PropertyType.GetInterface("IEnumerable") != null)
                    {
                        var coppiedCollection = (IList)prop.GetValue(coppied);
                        if (coppiedCollection != null && coppiedCollection.Count > 0)
                        {
                            var originalCollection = (IList)prop.GetValue(original);
                            for (int i = 0; i <= coppiedCollection.Count - 1; i++)
                            {
                                UpdateStateToEachProperty((BusinessBase)originalCollection[i], (BusinessBase)coppiedCollection[i]);
                            }
                        }
                    }
                    else
                    {
                        BusinessBase coppiedValue = prop.GetValue(coppied) as BusinessBase;
                        if (coppiedValue != null)
                        {
                            BusinessBase originalValue = prop.GetValue(original) as BusinessBase;
                            UpdateStateToEachProperty(originalValue, coppiedValue);
                            if (!originalValue.IsSelfDirty) coppiedValue.MarkAsSelfOld();
                        }
                    }
                }
            }
        }

        private void CopyValueToEachProperty<R>(R stateObject)
                   where R : BusinessBase
        {
            IList<PropertyInfo> props = new List<PropertyInfo>(GetType().GetProperties());
            foreach (PropertyInfo prop in props)
            {
                if (prop.CanWrite)
                    prop.SetValue(this, prop.GetValue(stateObject));
            }

            if (stateObject.IsNew)
                this.MarkAsNew();
            if (stateObject.IsDeleted)
                this.MarkAsDeleted();
            if (!stateObject.IsSelfDirty)
                this.MarkAsSelfOld();
        }

        #endregion Functions

        protected class BusinessRuleList
        {
            internal List<KeyValuePair<ValidationDelegate, string>> _businessRules = new List<KeyValuePair<ValidationDelegate, string>>();

            public void AddRules(ValidationDelegate rule, string message)
            {
                _businessRules.Add(new KeyValuePair<ValidationDelegate, string>(rule, message));
            }
        }
    }
}