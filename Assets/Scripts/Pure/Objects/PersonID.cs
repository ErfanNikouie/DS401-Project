using System;

[Serializable]
public struct PersonID : IID
{
	private int value;
	public int Value => value;

	public PersonID(int value) => this.value = value;
	public bool Equals(IID id) => Value == id.Value;
}
