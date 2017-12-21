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
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Type = NokProjectX.Wpf.Entities.Type;

    /// <summary>
    /// Defines the <see cref="AddProductViewModel" />
    /// </summary>
    public class AddProductViewModel : ValidatableViewModelBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _description
        /// </summary>
        private string _description;

        /// <summary>
        /// Defines the _image
        /// </summary>
        private byte _image;

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
        private string _price;

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
        private string _stock;

        /// <summary>
        /// Defines the _type
        /// </summary>
        private string _type;

        /// <summary>
        /// Defines the _types
        /// </summary>
        private List<Type> _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public AddProductViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();
            Types = _context.Types.ToList();
            ConfigureValidationRules();
        }

        /// <summary>
        /// Gets or sets the AddCommand
        /// </summary>
        public RelayCommand AddCommand { get; set; }

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
        /// Gets or sets the Image
        /// </summary>
        public byte Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }

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
        public string Price
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
        public string Stock
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
            Validator.AddRule(nameof(ProductName),
                () =>
                {
                    var count = _context.Products.Count(c => c.Name.ToLower().Equals(ProductName.Trim().ToLower()));
                    var result = count == 0;
                    return RuleResult.Assert(result,
                        $"Product already exists");
                });

            Validator.AddRequiredRule(() => Description, "Description is required");

            Validator.AddRequiredRule(() => SelectedType, "Type is required");

            Validator.AddRequiredRule(() => Stock, "Stock is required");

            Validator.AddRule(nameof(Stock),
                () =>
                {
                    int num;
                    var result = int.TryParse(Stock, out num);
                    return RuleResult.Assert(result,
                        $"Stock must be a number.");
                });

            Validator.AddRequiredRule(() => Price, "Price is required");

            Validator.AddRule(nameof(Price),
                () =>
                {
                    double num;
                    var result = double.TryParse(Price, out num);
                    return RuleResult.Assert(result,
                        $"Price must be a number.");
                });
        }

        /// <summary>
        /// The LoadCommands
        /// </summary>
        private void LoadCommands()
        {
            ViewCommand = new RelayCommand(OnView);
            AddCommand = new RelayCommand(OnAdd);
            CloseCommand = new RelayCommand(OnClose);
            UploadCommand = new RelayCommand(OnUpload);
        }

        /// <summary>
        /// The OnAdd
        /// </summary>
        private async void OnAdd()
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }
            var codeNumber = _context.Products.Select(c => c.CodeNumber).OrderByDescending(c => c).FirstOrDefault();
            int finalNumber = 0;
            if (codeNumber > 0)
            {
                finalNumber = codeNumber + 1;
            }
            else
            {
                finalNumber = 1000001;
            }
            string codeString = ProductName.Substring(0, 4).ToUpper();
            
            //var picture = ImageToByteArray(Picture); 
            Product newProduct = new Product()
            {
                CodeNumber = finalNumber,
                CodeString = codeString,
                Name = ProductName,
                Description = Description,
                Type = SelectedType,
                Stock = Int32.Parse(Stock),
                Price = Double.Parse(Price),
                Image = Picture
            };
            _context.Products.Add(newProduct);
            _context.SaveChanges();
            MessengerInstance.Send(new RefreshMessage());

            OnClose();
        }

        //        public Image ByteArrayToImage(byte[] byteArrayIn)
        //        {
        //            MemoryStream ms = new MemoryStream(byteArrayIn);
        //            var returnImage = System.Drawing.Image.FromStream(ms);
        //            return returnImage;
        //        }
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
        /// The OnUpload
        /// </summary>
        private void OnUpload()
        {
            var openFileDialog = new OpenFileDialog { Filter = "Image Files (JPG,PNG)|*.JPG;*.PNG" };
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

        /// <summary>
        /// The Validate
        /// </summary>
        private async void Validate()
        {
            await ValidateAsync();
        }
    }
}
