namespace Library.DataAccess;
public interface ICollectionData<Model> {
    public Task<List<Model>> GetAll();
}
