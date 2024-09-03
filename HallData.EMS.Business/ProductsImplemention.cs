using HallData.EMS.ApplicationViews;
using HallData.EMS.Data;
using HallData.Security;

namespace HallData.EMS.Business
{
	public class ReadOnlyProductImplementation : ReadOnlyProductBaseImplementation<IReadOnlyProductRepository, ProductResult>, IReadOnlyProductImplementation
	{

		public ReadOnlyProductImplementation(IReadOnlyProductRepository repository, ISecurityImplementation security) :
			base(repository, security)
		{
		}

	}

	public class ProductImplementation : 
		ProductBaseImplementation<IProductRepository, IReadOnlyProductImplementation, ProductResult, ProductForAdd, ProductForUpdate>, IProductImplementation
	{
		protected IProductContactImplementation ProductContact { get; private set; }
		public ProductImplementation(IProductRepository repository, ISecurityImplementation security, IProductContactImplementation productContact, IReadOnlyProductImplementation readOnly)
			: base(repository, security, readOnly)
		{
			this.ProductContact = productContact;
		}
	}
}