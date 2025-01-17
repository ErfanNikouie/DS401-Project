using System.Collections.Generic;

public interface IPool<T>
{
	public List<T> Pool { get; }
	public bool Add(T value);
	public bool ValidateID(IID id);
	public IID GenerateUniqueID();
	public T Lookup(IID id);
}
