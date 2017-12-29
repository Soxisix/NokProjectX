namespace NokProjectX.Wpf.ViewModel.Inventory
{
    using GalaSoft.MvvmLight.Command;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Win32;
    using MvvmValidation;
    using NokProjectX.Wpf.Common.Messages;
    using NokProjectX.Wpf.Common.Validator;
    using NokProjectX.Wpf.Context;
    using NokProjectX.Wpf.Entities;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Type = NokProjectX.Wpf.Entities.Type;

    /// <summary>
    /// Defines the <see cref="EditProductViewModel" />
    /// </summary>
    public class EditProductViewModel : ValidatableViewModelBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _currentProduct
        /// </summary>
        private Product _currentProduct;

        /// <summary>
        /// Defines the _description
        /// </summary>
        private string _description;

        /// <summary>
        /// Defines the _isOpen
        /// </summary>
        private bool _isOpen;

        /// <summary>
        /// Defines the _picture
        /// </summary>
        private byte[] _picture;

        /// <summary>
        /// Defines the _price
        /// </summary>
        private double? _price;

        /// <summary>
        /// Defines the _productName
        /// </summary>
        private string _productName;

        /// <summary>
        /// Defines the _selectedType
        /// </summary>
        private Type _selectedType;

        /// <summary>
        /// Defines the _stock
        /// </summary>
        private double? _stock;

        /// <summary>
        /// Defines the _type
        /// </summary>
        private string _type;

        /// <summary>
        /// Defines the _types
        /// </summary>
        private List<Type> _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditProductViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public EditProductViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();
            Types = _context.Types.ToList();
            ConfigureValidationRules();
            MessengerInstance.Register<SelectedProductMessage>(this, OnProductRecieve);
        }

        /// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public RelayCommand CloseCommand { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        /// <summary>
        /// Gets or sets the EditCommand
        /// </summary>
        public RelayCommand EditCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsOpen
        /// </summary>
        public bool IsOpen
        {
            get { return _isOpen; }
            set { Set(ref _isOpen, value); }
        }

        /// <summary>
        /// Gets or sets the Picture
        /// </summary>
        public byte[] Picture
        {
            get { return _picture; }
            set { Set(ref _picture, value); }
        }

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public double? Price
        {
            get { return _price; }
            set { Set(ref _price, value); }
        }

        /// <summary>
        /// Gets or sets the ProductName
        /// </summary>
        public string ProductName
        {
            get { return _productName; }
            set { Set(ref _productName, value); }
        }

        /// <summary>
        /// Gets or sets the SelectedType
        /// </summary>
        public Type SelectedType
        {
            get { return _selectedType; }
            set { Set(ref _selectedType, value); }
        }

        /// <summary>
        /// Gets or sets the Stock
        /// </summary>
        public double? Stock
        {
            get { return _stock; }
            set { Set(ref _stock, value); }
        }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { Set(ref _type, value); }
        }

        /// <summary>
        /// Gets or sets the Types
        /// </summary>
        public List<Type> Types
        {
            get { return _types; }
            set { Set(ref _types, value); }
        }

        /// <summary>
        /// Gets or sets the UploadCommand
        /// </summary>
        public RelayCommand UploadCommand { get; set; }

        /// <summary>
        /// Gets or sets the ViewCommand
        /// </summary>
        public RelayCommand ViewCommand { get; set; }

        /// <summary>
        /// The ImageToByteArray
        /// </summary>
        /// <param name="imageIn">The <see cref="Image"/></param>
        /// <returns>The <see cref="byte[]"/></returns>
        public byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }

        //        private async void Validate()
        //        {
        //            await ValidateAsync();
        //        }
        /// <summary>
        /// The ValidateAsync
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task ValidateAsync()
        {
            await Validator.ValidateAllAsync();
        }

        /// <summary>
        /// The Clear
        /// </summary>
        internal void Clear()
        {
            ProductName = null;
            Description = null;
            SelectedType = null;
            Stock = null;
            Price = null;
        }

        /// <summary>
        /// The ConfigureValidationRules
        /// </summary>
        private void ConfigureValidationRules()
        {
            //            Validator.AddAsyncRule(nameof(LRN),
            //                async () =>
            //                {
            //                    var _context = new MorenoContext();
            //                    var result = await _context.Students.FirstOrDefaultAsync(e => e.LRN == LRN);
            //                    bool isAvailable = result == null;
            //                    return RuleResult.Assert(isAvailable,
            //                        string.Format("LRN {0} is taken. Please choose a different one.", LRN));
            //                });
            Validator.AddRequiredRule(() => ProductName, "Product Name is required");
            //            Validator.AddAsyncRule(nameof(ProductName),
            //                validateAction: async () =>
            //                {
            //                    var count = _context.Products.Count(c => c.Name.ToLower().Equals(ProductName.Trim().ToLower()));
            //                    var result = count == 0;
            //                    return RuleResult.Assert(result,
            //                        $"Product already exists");
            //                });

            Validator.AddRequiredRule(() => Description, "Description is required");

            Validator.AddRequiredRule(() => SelectedType, "Type is required");

            Validator.AddRequiredRule(() => Stock, "Stock is required");

            //            Validator.AddAsyncRule(nameof(Stock),
            //                validateAction: async () =>
            //                {
            //                    int num;
            //                    var result = int.TryParse(Stock, out num);
            //                    return RuleResult.Assert(result,
            //                        $"Stock must be a number.");
            //                });

            Validator.AddRequiredRule(() => Price, "Price is required");
        }

        /// <summary>
        /// The LoadCommands
        /// </summary>
        private void LoadCommands()
        {
            ViewCommand = new RelayCommand(OnView);
            EditCommand = new RelayCommand(OnEdit);
            CloseCommand = new RelayCommand(OnClose);
            UploadCommand = new RelayCommand(OnUpload);
        }

        /// <summary>
        /// The OnClose
        /// </summary>
        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
            Clear();
            IsOpen = false;
            Validator.Reset();
        }

        /// <summary>
        /// The OnEdit
        /// </summary>
        private async void OnEdit()
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }
            //            int newCode = 0;
            //            var result = _context.Products.Select(c => c.ProductCode).OrderByDescending(c => c).FirstOrDefault();
            //            if (result != null)
            //            {
            //                newCode = result + 1;
            //            }
            //            Product newProduct = new Product()
            //            {
            //                Name = ProductName,
            //                Description = Description,
            //                Type = SelectedType,
            //                Stock = Int32.Parse(Stock),
            //                Price = Double.Parse(Price)
            //            };
            //            _context.Products.AddOrUpdate(newProduct);
            var product = _context.Products.FirstOrDefault(c => c.CodeString + c.CodeNumber == _currentProduct.ProductCode);
            if (product != null)
            {
                product.Name = ProductName;
                product.Description = Description;
                product.Type = SelectedType;
                product.Stock = Stock.GetValueOrDefault();
                product.Price = Price.GetValueOrDefault();
                product.Image = Picture;
            }
            _context.SaveChanges();
            MessengerInstance.Send(new RefreshMessage());

            OnClose();
        }

        /// <summary>
        /// The OnProductRecieve
        /// </summary>
        /// <param name="obj">The <see cref="SelectedProductMessage"/></param>
        private void OnProductRecieve(SelectedProductMessage obj)
        {
            _currentProduct = obj.SelectedProduct;
            if (obj.SelectedProduct == null)
            {
                return;
            }
            ProductName = _currentProduct.Name;
            Description = _currentProduct.Description;
            Stock = _currentProduct.Stock;
            SelectedType = _currentProduct.Type;
            Price = _currentProduct.Price;
            Picture = _currentProduct.Image;
        }

        /// <summary>
        /// The OnUpload
        /// </summary>
        private void OnUpload()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Image Files (JPG,PNG)|*.JPG;*.PNG" };
            openFileDialog.ShowDialog();
            if (!openFileDialog.CheckFileExists)
            {
                MessageBox.Show("File not exist");
                return;
            }

            if (Path.GetExtension(openFileDialog.FileName) == ".jpg" ||
                Path.GetExtension(openFileDialog.FileName) == ".png")
            {
                //                MessageBox.Show((openFileDialog.FileName));

                Picture = ImageToByteArray(new Bitmap(openFileDialog.FileName));
            }
        }

        /// <summary>
        /// The OnView
        /// </summary>
        private void OnView()
        {
            if (IsOpen)
            {
                IsOpen = false;
                return;
            }
            IsOpen = true;
        }
    }
}
