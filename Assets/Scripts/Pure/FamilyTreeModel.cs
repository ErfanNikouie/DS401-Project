using System.Collections.Generic;

public class FamilyTreeModel
{
	private PersonPool people = new PersonPool();
	private FamilyPool families = new FamilyPool();

	private Dictionary<int, FamiliesInvolved> familiesInvolvedDict = new Dictionary<int, FamiliesInvolved>();

	public PersonPool People => people;
	public FamilyPool Families => families;

	public int PeopleCount => people.Pool.Count;
	public int FamilyCount => families.Pool.Count;
	
	
	#region Helper State Functions

	public bool IsPeopleEmpty() => people?.Pool?.Count == 0;
	public bool IsFamiliesEmpty() => families?.Pool?.Count == 0;
	public bool IsFamiliesInvolvedEmpty() => familiesInvolvedDict?.Count == 0;
	public bool IsEmpty() => IsPeopleEmpty() && IsFamiliesEmpty() && IsFamiliesInvolvedEmpty();

	public PersonID GetUniquePersonID() => (PersonID)people.GenerateUniqueID();
	public FamilyID GetUniqueFamilyID() => (FamilyID)families.GenerateUniqueID();
	
	public PersonID ContainsPerson(Person person)
	{
		for (int i = 0; i < people.Pool.Count; i++)
			if (people.Pool[i].IsEqual(person))
				return new PersonID(i);

		return new PersonID(-1);
	}
	
	public FamilyID ContainsFamily(Family family)
	{
		for (int i = 0; i < families.Pool.Count; i++)
			if (families.Pool[i].IsEqual(family))
				return new FamilyID(i);

		return new FamilyID(-1);
	}

	public Person LookupPerson(PersonID person) => people.Lookup(person);

	public Family LookupFamily(FamilyID family) => families.Lookup(family);

	public bool TryGetFamiliesInvolved(PersonID person, out FamiliesInvolved result) => familiesInvolvedDict.TryGetValue(person.Value, out result);
	
	#endregion
	
	#region Validation

	public bool ValidatePersonID(IID id) => people.ValidateID(id);

	public bool ValidateFamilyID(IID id) => families.ValidateID(id);
	
	#endregion
	
	public void AddFamiliesInvolvedAsChild(PersonID person, FamilyID family)
	{
		if (!familiesInvolvedDict.ContainsKey(person.Value))
			familiesInvolvedDict[person.Value] = new FamiliesInvolved();
		
		familiesInvolvedDict[person.Value].ChildOf = family;
	}
	
	public void AddFamiliesInvolvedAsOwner(PersonID person, FamilyID family)
	{
		if (!familiesInvolvedDict.ContainsKey(person.Value))
			familiesInvolvedDict[person.Value] = new FamiliesInvolved();

		familiesInvolvedDict[person.Value].OwnerOf = family;
	}

	public bool AddPerson(Person person) => people.Add(person);

	public bool AddFamily(Family family)
	{
		if (!ValidatePersonID(family.Father) || !ValidatePersonID(family.Mother)) return false;

		FamilyID fid = GetUniqueFamilyID();
		
		AddFamiliesInvolvedAsOwner(family.Father, fid);
		AddFamiliesInvolvedAsOwner(family.Mother, fid);
		return families.Add(family);
	}
	
	public bool AddChildToFamily(PersonID child, FamilyID family)
	{
		if (!ValidatePersonID(child) || !ValidateFamilyID(family)) return false;
		
		AddFamiliesInvolvedAsChild(child, family);
		return LookupFamily(family).AddChild(child);
	}
}
