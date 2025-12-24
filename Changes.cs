/*
 * Created by SharpDevelop.
 * User: afisch
 * Date: 18.03.2012
 * Time: 11:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace AFUG.Changes
{
	/// <summary>
	/// Wird verwendet um Änderungen in den Daten nachzuverfolgen.
	/// </summary>
	public enum ChangeEnum { HasNotChanged = 0, HasChanged = 1, HasBeenCreated = 2, HasBeenDeleted = 4, HasBeenAdded = 8, HasBeenRemoved = 16, Unknown = 32 }

	/// <summary>
	/// Interface zur Implementierung in Klassen, die eine Nachverfolgung von Änderungen ermöglichen.
	/// </summary>
	public interface IChangesTrackable
	{
		ChangeEnum ChangeState { get; }
	}
}