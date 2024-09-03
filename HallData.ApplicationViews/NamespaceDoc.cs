using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.ApplicationViews
{
    /// <summary>
    /// <para>
    /// The HallData.ApplicationViews namespace contains interfaces and attributes to help the base framework interact with application views.
    /// </para>
    /// </summary>
    /// <remarks>
    /// An Application View is a class representing data returned, or consumed, by a stored procedure. It does not map one to one with the database tables and therefor is not an entity in the traditional sense.
    /// </remarks>
    /// <example>
    /// <para>The following example demonstrates how a complex application view should be constructed.</para>
    /// <code>
    /// public class FooKey : IHasKey&lt;Guid&gt;
    /// {
    ///     [ChildKeyOperationParameter]
    ///     [UpdateOperationParameter]
    ///     public Guid? FooId{get;set;} //should be nullable for adds
    ///     
    ///     [JsonIgnore] //This should not be serialized
    ///     public Guid Key
    ///     {
    ///         get{return this.FooId ?? Guid.Empty;} //The key should be non-nullable, return empty key for null
    ///         set{this.FooId = value;}
    ///     }
    /// }
    /// public class FooBase&lt;TBar, TCategory&gt; : FooKey
    ///     where TBar : BarKey
    ///     where TCategory: CategoryKey
    /// {
    ///     public FooBase()
    ///     {
    ///         this.Categories = new HashSet&lt;TCategory&gt;();
    ///     }
    ///     [UpdateOperationParameter]
    ///     [AddOperationParameter]
    ///     public string Name{get;set;}
    ///     [ChildView]
    ///     public TBar Bar{get;set;}
    ///     [UpdateOperationParameter]
    ///     [AddOperationParameter]
    ///     [ChildViewCollection("dbo.categorykeytabletype")]
    ///     public ICollection&lt;TCategory&gt; Categories{get;set;}
    /// }
    /// public class FooBase : FooBase&lt;BarKey, CategoryKey&gt;{} //class used for consumption
    /// public class FooResult: FooBase&lt;Bar, Category&gt;{} //class used for results
    /// public class BarKey : IHasKey
    /// {
    ///     [ChildKeyOperationParameter]
    ///     [UpdateOperationParameter]
    ///     public int? BarId{get;set;}
    ///     
    ///     [JsonIgnore]
    ///     public int Key
    ///     {
    ///         get{return this.BarId ?? 0;}
    ///         set{this.BarId = value;}
    ///     }
    /// }
    /// public class Bar : BarKey
    /// {
    ///     [UpdateOperationParameter]
    ///     [AddOperationParameter]
    ///     public string Name{get;set;}
    /// }
    /// public class CategoryKey: IHasKey
    /// {
    ///     [ChildKeyOperationParameter]
    ///     [UpdateOperationParameter]
    ///     public int? CategoryId{get;set;}
    ///     
    ///     [JsonIgnore]
    ///     public int Key
    ///     {
    ///         get{return this.CategoryId ?? 0;}
    ///         set{this.CategoryId = value;}
    ///     }
    /// }
    /// public class Category : CategoryKey
    /// {
    ///     [UpdateOperationParameter]
    ///     [AddOperationParameter]
    ///     public string Name{get;set;}
    /// }
    /// </code>
    /// <para>
    /// If update is called on FooBase the following parameters will be mapped:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Path</term>
    /// <term>Parameter Name</term>
    /// <term>Parameter Type</term>
    /// </listheader>
    /// <item>
    /// <term>FooId</term>
    /// <term>FooId</term>
    /// <term>Guid</term>
    /// </item>
    /// <item>
    /// <term>Name</term>
    /// <term>Name</term>
    /// <term>string</term>
    /// </item>
    /// <item>
    /// <term>Bar.BarId</term>
    /// <term>Bar_BarId</term>
    /// <term>int</term>
    /// </item>
    /// <item>
    /// <term>Categories</term>
    /// <term>Categories</term>
    /// <term>DataTable[CategoryId int]</term>
    /// </item>
    /// </list>
    /// <para>
    /// If add is called on FooBase the following parameters will be mapped:
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Path</term>
    /// <term>Parameter Name</term>
    /// <term>Parameter Type</term>
    /// </listheader>
    /// <item>
    /// <term>Name</term>
    /// <term>Name</term>
    /// <term>string</term>
    /// </item>
    /// <item>
    /// <term>Bar.BarId</term>
    /// <term>Bar_BarId</term>
    /// <term>int</term>
    /// </item>
    /// <item>
    /// <term>Categories</term>
    /// <term>Categories</term>
    /// <term>DataTable[CategoryId int]</term>
    /// </item>
    /// </list>
    /// </example>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class NamespaceDoc
    {
    }
}
