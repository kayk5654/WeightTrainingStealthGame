/// <summary>
/// link a class in the domain layer with relative class instance in the scene
/// </summary>
/// <typeparam name="T">parent class in the domain layer</typeparam>
/// <typeparam name="U">child class inherits MonoBehaviour in the presentation layer</typeparam>
public interface ISceneObjectLinker<T>
{
    /// <summary>
    /// link parent object and relative objects
    /// </summary>
    /// <param name="parentObject"></param>
    void LinkObject(T parentObject);
}
