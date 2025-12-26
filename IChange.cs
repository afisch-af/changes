/*
 * Created by SharpDevelop.
 * User: fischera
 * Date: 07.09.2017
 * Time: 10:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace af.Changes
{
	/// <summary>
	/// Description of IChange.
	/// </summary>
	public interface IChange
	{
		ChangeStateEnum ChangeState{ get; }
		void ProcessChange();
		void AcceptChanges();
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class ChangesTrackedAttribute : Attribute
	{
	}
	
	/// <summary>
	/// Beschreibt den Status eines Objekts.
	/// </summary>
	public enum ChangeStateEnum {
		isNew,
		hasChanged,
		hasNotChanged,
		empty
	}
	
	public static class Process{
		public static ChangeStateEnum Change(ChangeStateEnum oldState) {
			if (oldState == ChangeStateEnum.isNew) {
				return ChangeStateEnum.isNew;
			}
			if (oldState == ChangeStateEnum.empty) {
				return ChangeStateEnum.isNew;
			} else {
				return ChangeStateEnum.hasChanged;
			}
		}
	}

	public abstract class ChangeTrackedBase : IChange
	{
		private ChangeStateEnum _changeState = ChangeStateEnum.empty;

		public ChangeStateEnum ChangeState { get { return _changeState; } }

		public void ProcessChange()
		{
			_changeState = Process.Change(_changeState);
		}

		public void AcceptChanges()
		{
			if (_changeState != ChangeStateEnum.empty) {
				_changeState = ChangeStateEnum.hasNotChanged;
			}
		}

		protected bool SetTrackedProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(field, value)) {
				return false;
			}

			field = value;

			if (IsTrackedProperty(propertyName)) {
				ProcessChange();
			}

			return true;
		}

		private bool IsTrackedProperty(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName)) {
				return false;
			}

			Type type = GetType();
			if (!Attribute.IsDefined(type, typeof(ChangesTrackedAttribute), true)) {
				return false;
			}

			PropertyInfo property = type.GetProperty(propertyName);
			if (property == null) {
				return false;
			}

			return Attribute.IsDefined(property, typeof(ChangesTrackedAttribute), true);
		}
	}
}
