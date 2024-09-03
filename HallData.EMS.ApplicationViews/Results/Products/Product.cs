
namespace HallData.EMS.ApplicationViews
{
    public class ProductKey : ProductBaseKey, IProductKey
    {
       
    }
    public class Product<TProductType> : ProductBase<TProductType>, IProduct<TProductType>
        where TProductType : ProductTypeKey
    {

    }
}
