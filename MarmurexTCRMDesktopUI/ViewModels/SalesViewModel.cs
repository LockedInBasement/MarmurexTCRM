using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MarmurexTCRMDesktopUI.Library.Api;
using MarmurexTCRMDesktopUI.Library.Models;
using MarmurexTCRMDesktopUI.Library.Helpers;
using AutoMapper;
using MarmurexTCRMDesktopUI.Models;
using System.Dynamic;
using System.Windows;

namespace MarmurexTCRMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
	{
		IProductEndpoint _productEndpoint;
		IConfigHelper _configHelper;
		ISaleEndPoint _saleEndPoint;
		IMapper _mapper;
		StatusInfoViewModel _status;
		IWindowManager _window;

		public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint, IMapper mapper, StatusInfoViewModel  status,IWindowManager window)
		{
			_productEndpoint = productEndpoint;
			_configHelper = configHelper;
			_saleEndPoint = saleEndPoint;
			_mapper = mapper;
			_status = status;
			_window = window;
		}

		public async Task LoadProducts()
		{
			var productList = await _productEndpoint.GetAll();
			var products = _mapper.Map<List<ProductDisplayModel>>(productList);

			Products = new BindingList<ProductDisplayModel>(products);
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			try
			{
				await LoadProducts();
			}
			catch (Exception ex)
			{
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;
				settings.Title = "System ERROR";

				if(ex.Message == "Unathorized")
				{
					_status.UpdateMessage("Unathorized", "Permission");
					_window.ShowDialog(_status, null, settings);
				}

				TryClose();
			}
		}

		private BindingList<ProductDisplayModel> _products;

		public BindingList<ProductDisplayModel> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductDisplayModel _selectedProduct;

		public ProductDisplayModel SelectedProduct
		{
			get { return _selectedProduct; }
			set
			{
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		private CartItemDisplayModel _selectedCartItem;

		public CartItemDisplayModel SelectedCartItem
		{
			get { return _selectedCartItem; }
			set
			{
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
		}

		private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

		public BindingList<CartItemDisplayModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		private int _itemQuantity = 1;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public string SubTotal
		{
			get
			{
				return CalculateSubTotal().ToString("C");
			}
		}

		private decimal CalculateSubTotal()
		{
			decimal subTotal = 0;

			foreach (var item in Cart)
			{
				subTotal += (item.Product.RetailPrice * item.QuantityInCart);
			}

			return subTotal;
		}

		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTax();
				return total.ToString("C");
			}
		}

		public string Tax
		{
			get
			{
				return CalculateTax().ToString("C");
			}
		}

		private decimal CalculateTax()
		{
			decimal taxTotal = 0;
			decimal taxRate = _configHelper.GetTaxRate()/100;

			taxTotal = Cart.
				Where(x => x.Product.IsTaxable).
				Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);


			//foreach (var item in Cart)
			//{
			//	if (item.Product.IsTaxable)
			//	{
			//		taxTotal += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
			//	}
			//}

			return taxTotal;
		}

		public bool CanAddToCart
		{
			get
			{
				bool output = false;

				if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}

				return output;
			}
		}

		public void AddToCart()
		{
			CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

			if(existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
			}
			else
			{
				CartItemDisplayModel item = new CartItemDisplayModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};

				Cart.Add(item);
			}

			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				if(SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public void RemoveFromCart()
		{
			SelectedCartItem.Product.QuantityInStock += 1;

			if (SelectedCartItem.QuantityInCart > 1)
			{
				SelectedCartItem.QuantityInCart -= 1;
			}
			else
			{
				Cart.Remove(SelectedCartItem);
			}

			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
			NotifyOfPropertyChange(() => CanAddToCart);
		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				if(Cart.Count > 0)
				{
					output = true;
				}

				return output;
			}
		}

		private async Task ResetSalesViewModel()
		{
			Cart = new BindingList<CartItemDisplayModel>();

			await LoadProducts();

			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public async Task CheckOut()
		{
			SaleModel sale = new SaleModel();

			foreach (var item in Cart)
			{
				sale.saleDetails.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}

			await _saleEndPoint.PostSale(sale);

			await ResetSalesViewModel();
		}
	}
}
