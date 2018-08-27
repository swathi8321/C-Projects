using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShopUI
{
    public partial class ConsignmentShop : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();

        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorBinding = new BindingSource();

        private decimal storeProfit = 0;

        public ConsignmentShop()
        {
            InitializeComponent();
            SetupData();
            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsListBox.DataSource = itemsBinding;

            itemsListBox.DisplayMember = "Display";
            itemsListBox.ValueMember = "Display";

            cartBinding.DataSource = shoppingCartData;
            shoppingCartListBox.DataSource = cartBinding;

            shoppingCartListBox.DisplayMember = "Display";
            shoppingCartListBox.ValueMember = "Display";

            vendorBinding.DataSource = store.Vendors;
            vendorListBox.DataSource = vendorBinding;

            vendorListBox.DisplayMember = "Display";
            vendorListBox.ValueMember = "Display";


        }

        private void SetupData()
        {
            store.Vendors.Add(new Vendor{ FirstName = "Bill",LastName = "Smith"});
            store.Vendors.Add(new Vendor { FirstName = "Sue", LastName = "Jones"});

            store.Items.Add(new Item
            {
                Title = "Moby Dick",
                Description = "A book about whale",
                Price = 4.59M,
                Owner = store.Vendors[0]
            });

            store.Items.Add(new Item
            {
             Title = "Tale of 2 cities",
              Description = "A book about revolution",
              Price = 3.80M,
             Owner = store.Vendors[1]
            });
            store.Items.Add(new Item
            {
                Title = "Harry Potter",
                Description = "A book about boy",
                Price = 7.90M,
                Owner = store.Vendors[1]
            });
            store.Items.Add(new Item
            {
                Title = "Nancy Drew",
                Description = "A book about detective girl",
                Price = 9.80M,
                Owner = store.Vendors[0]
            });

            store.Name = "Seconds are Better";

        }

        private void addToCart_Click(object sender, EventArgs e)
        {
           Item selectedItem = (Item)itemsListBox.SelectedItem;
           shoppingCartData.Add(selectedItem);

            cartBinding.ResetBindings(false);
        }

        private void MakePurchase_Click(object sender, EventArgs e)
        {
            //Mark each item in the cart as sold
            //Clear the cart

            foreach(Item item in shoppingCartData)
            {
                item.Sold = true;
                item.Owner.PaymentDue += (decimal)item.Owner.Commision * item.Price;
                storeProfit += (1 - (decimal)item.Owner.Commision) * item.Price;

            }
            shoppingCartData.Clear();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            storeProfitText.Text = string.Format("${0}",storeProfit);

            itemsBinding.ResetBindings(false);
            cartBinding.ResetBindings(false);
            vendorBinding.ResetBindings(false);
        }
    }
}
