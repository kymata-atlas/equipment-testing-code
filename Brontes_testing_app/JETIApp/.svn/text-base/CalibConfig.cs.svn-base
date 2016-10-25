using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Reflection;
using Yao = Yaowi.Common.Serialization;
using System.Xml.Serialization;

using System.IO;

namespace JETIApp
{

	[Flags]
	public enum ExtraBitsEnum
	{
		_001 = 1,
		_100 = 2,
		_010 = 4,
		_101 = 8,
		_011 = 16,
		_110 = 32
	}

    public enum BrontesGainEnum
    {
        Auto=0,
        Highest_1=1,
        _2=2,
        _3=3,
        _4=4,
        _5=5,
        _6=6,
        _7=7,
        Lowest_8=8
    }

	public enum SpanSideEnum
	{
		Full,	
		Left,
		Right,
	}

	public enum TargetDeviceEnum
	{
		Not_Set,
		Software_2_2,
		CRS_Spectrocal,
		Minolta_LS110,
		Brontes//,
		//BrontesLibUSB

	}


	public enum TargetColourEnum
	{
		Not_Set,
		Grayscale,
		Red,
		Green,
		Blue
	}

    [Serializable]
	public class CalibConfig : Component, INotifyPropertyChanged
    {
        private string _OutputFile;
		private bool _WriteOutput;

        private uint _DisplayWidth;
        private uint _DisplayHeight;

        private uint _PatchWidth;
        private uint _PatchHeight;

        private uint _TotalReadings;
        private uint _ReadingsPerLevel;

        private uint _TotalReadingsDone;
        private uint _ReadingsDonePerLevel;

        private uint _GrayLevelStepSize;
		private uint _NoOfGrayLevels;

        private List<uint> _ScreenNumbers;
		private bool _SpanScreens;
		private SpanSideEnum _SpanSide;

		private bool _ExtendRange;

        private uint _StartDelay;
		private uint _DelayBetweenLevels;

		private bool _WriteTimings;

		private bool _EmailWhenComplete;

		private string _SMTPServer;

		private string _EmailTo;

		private bool _DeferCalc;

		private bool _RandomizeRepeats;

		private TargetDeviceEnum _TargetDevice;

		internal static uint[] _GrayLevelSteps ={ 1, 2, 4, 8, 16, 32, 64, 128, 255 };
		
		private ExtraBitsEnum _ExtraBits;

		private TargetColourEnum _TargetColour;

		private string _GammaFile;

		[Category("Configuration"),Description("Repeat readings are randomized rather than consecutive if this is true")]
		public bool RandomizeRepeats
		{
			get
			{
				return _RandomizeRepeats;
			}
			set
			{
				_RandomizeRepeats=value;
                NotifyPropertyChanged("RandomizeRepeats");
				CalcTotalReadings();
			}
		}

		[Category("Configuration"),Description("Target device to perform calibration")]
		public TargetDeviceEnum TargetDevice
		{
			get
			{
				return _TargetDevice;
			}
			set
			{
				_TargetDevice=value;
				NotifyPropertyChanged("TargetDevice");
			}
		}

        private int _IntegrationTime;
        [Category("Brontes"),Description("Integration Time in microseconds")]
        public int IntegrationTime
        {
            get
            {
                return _IntegrationTime;
            }
            set
            {
                _IntegrationTime=value;
                NotifyPropertyChanged("IntegrationTime");
            }

        }

        private BrontesGainEnum _Gain;
        [Category("Brontes"),Description("Gain: 0 = Auto, 1 = Highest, 8 = Lowest")]
        public BrontesGainEnum Gain
        {
            get
            {
                return _Gain;
            }
            set
            {
                _Gain=value;
                NotifyPropertyChanged("Gain");
            }
        }
           

        private int _Samples;
        [Category("Brontes"),Description("No of samples to average over")]
        public int Samples
        {
            get
            {
                return _Samples;
            }
            set
            {
                _Samples=value;
                NotifyPropertyChanged("Samples");
            }
        }

		[Category("Configuration"),Description("Target colour")]
		public TargetColourEnum TargetColour
		{
			get
			{
				return _TargetColour;
			}
			set
			{
				_TargetColour = value;
                NotifyPropertyChanged("TargetColour");
			}
		
		}


		[Category("Information"), ReadOnly(true), Description("In horizontal span mode use left or right side of display")]
        [XmlIgnore]
		public SpanSideEnum SpanSide
		{
			get
			{
				return _SpanSide;
			}
			set
			{
				_SpanSide = value;
                NotifyPropertyChanged("SpanSide");
			}
		}

		[Category("Email"), Description("Notify by e-mail when measurement is complete")]
		public bool EmailWhenComplete
		{
			get
			{
				return _EmailWhenComplete;
			}
			set
			{
				_EmailWhenComplete = value;
                NotifyPropertyChanged("EmailWhenComplete");
			}
		}

		[Category("Email"), Description("SMTP Server to use for notification e-mail")]
		public string SMTPServer
		{
			get
			{
				return _SMTPServer;
			}
			set
			{
				_SMTPServer = value;
                NotifyPropertyChanged("SMTPServer");
			}
		}

        private string _SMTPUser;
        [Category("Email"), Description("SMTP Server username")]
        public string SMTPUser
        {
            get
            {
                return _SMTPUser;
            }
            set
            {
                _SMTPUser = value;
                NotifyPropertyChanged("SMTPUser");
            }
        }

        private string _SMTPPassword;
        [Category("Email"), Description("SMTP Server password"),PasswordPropertyText(true)]
        [XmlIgnore]
        public string SMTPPassword
        {
            get
            {
                return "";
            }
            set
            {
                _SMTPPassword = value;
                NotifyPropertyChanged("SMTPPassword");
            }
        }

        public string GetSMTPPassword()
        {
            return _SMTPPassword;
        }

        private int _SMTPPort;
        [Category("Email"),Description("SMTP Port")]
        public int SMTPPort
        {
            get
            {
                return _SMTPPort;
            }
            set
            {
                _SMTPPort = value;
                NotifyPropertyChanged("SMTPPort");
            }
        }

		[Category("Email"), Description("Email address to send notification e-mails to")]
		public string EmailTo
		{
			get
			{
				return _EmailTo;
			}
			set
			{
				_EmailTo = value;
                NotifyPropertyChanged("EmailTo");
			}
		}
		
		[Category("Configuration"),Description("Write timing information to output file")]
		public bool WriteTimings
		{
			get
			{
				return _WriteTimings;
			}
			set
			{
				_WriteTimings = value;
                NotifyPropertyChanged("WriteTimings");
			}
		}

		[Category("Configuration"), Description("Write information to output file")]
		public bool WriteOutput
		{
			get
			{
				return _WriteOutput;
			}
			set
			{
				_WriteOutput = value;
                NotifyPropertyChanged("WriteOutput");
			}
		}

		[Category("Information"), Description("Number of gray levels based on step size - including extended range")]
		public uint NoOfGrayLevels
		{
			get
			{
				return _NoOfGrayLevels;
			}
		}

		private void CalcTotalReadings()
		{
			if (_DeferCalc == true)
				return;
			// determine total number of readings to take
			
			// determine if we are extending the range and by how many extra values
			uint ExtraValues=0;

			_NoOfGrayLevels = ((Constants.MaxLevel + 1) / _GrayLevelStepSize); // +1 because we have 256 levels
			
			if (_GrayLevelStepSize!=1)
				_NoOfGrayLevels=_NoOfGrayLevels+1; // add 0,0,0

			if (_ExtendRange == true)
			{
				Array ev;
				ev = Enum.GetValues(typeof(ExtraBitsEnum));
				foreach(ExtraBitsEnum i in Enum.GetValues(typeof(ExtraBitsEnum)))
				{
					if (_ExtraBits==(_ExtraBits | i)) // bit is set
						ExtraValues++;
				}

				_NoOfGrayLevels = NoOfGrayLevels + (ExtraValues * (_NoOfGrayLevels-1));

			}


			_TotalReadings = _ReadingsPerLevel * _NoOfGrayLevels;
			
			// remove readings for last level since we can't go past 255 levels
//			_TotalReadings=_TotalReadings - ExtraValues;

            NotifyPropertyChanged("TotalReadings");

		}

		[Category("Configuration"),Description("Extend the range of luminance values by setting these to true. For each option an extra reading per gray level will be made." +
				" e.g. For gray level 127,127,127 setting _001 will also take a reading with gray level 127,127,128 etc. Although the visual difference is negligible the luminance will be increased slightly which improves the linearity of the gamma table "),TypeConverter(typeof(FlagsEnumConverter))]
		public ExtraBitsEnum ExtraBits
		{
			get
			{
				return _ExtraBits;
			}
			set
			{
				_ExtraBits = value;
				if (_ExtraBits != 0)
				{
					_ExtendRange = true;
				}
				else
				{
					_ExtendRange = false;
				}

				NotifyPropertyChanged("ExtraBits");
                NotifyPropertyChanged("ExtendRange");

				CalcTotalReadings();

			}
		}


		[Category("Configuration"),Description("Delay before taking reading when gray level is changed in ms to allow measurement device to adjust to new gray level")]
		public uint DelayBeforeMeasurement
		{
			get
			{
				return _DelayBetweenLevels;
			}
			set
			{
				_DelayBetweenLevels=value;
                NotifyPropertyChanged("DelayBeforeMeasurement");
			}
		}

		[Category("Information"),Description("Specify if horizontal span mode should be assumed"),ReadOnly(true)]
        [XmlIgnore]
		public bool SpanScreens
		{
			get
			{
				return _SpanScreens;
			}
			set
			{
				_SpanScreens = value;
                NotifyPropertyChanged("SpanScreens");
			}
		}

		[Category("Information"), Description("Gamma lookup table to pre-load"), ReadOnly(true)]
		public string GammaFile
		{
			get
			{
				return _GammaFile;
			}
			set
			{
				_GammaFile = value;
                NotifyPropertyChanged("GammaFile");
			}
		}



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

		public CalibConfig()
		{
			_GammaFile = "";
			_DeferCalc = true;
		}
        public CalibConfig(uint width, uint height)
        {
			Init(width,height);
		}
				
		private void Init(uint width,uint height)
		{
            _DisplayWidth = width;
            _DisplayHeight = height;
            _PatchWidth = 600;
            _PatchHeight = 600;
            _GrayLevelStepSize = 64;
			_NoOfGrayLevels = ((Constants.MaxLevel +1) / _GrayLevelStepSize) + 1; //+1 to include 0,0,0
			_TotalReadings = _NoOfGrayLevels;
            _TotalReadingsDone = 0;
            _ReadingsDonePerLevel = 0;
			_GammaFile = "";
            _Gain=0;
            _Samples=10;
            _IntegrationTime = 20000;

			_WriteOutput = true;

			//int count = 1;
			//bool valid=false;
			//StringBuilder sb;
            _OutputFile = String.Format(@"{0}\Gamma Calibration Results {1}.lum",
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                System.DateTime.Now.ToShortDateString().Replace(@"/", "-"));
			if (File.Exists(_OutputFile))
            {
                bool valid = false;
                int count=1;
                string temp="";
                while (valid == false)
                {
                    temp = string.Format(@"{0}\{1} ({2}).lum", Path.GetDirectoryName(_OutputFile), Path.GetFileNameWithoutExtension(_OutputFile), count);
                    if (!File.Exists(temp))
                        valid = true;
                    else
                    {
                        count++;
                    }
                }
                _OutputFile = temp;
            }
			_ReadingsPerLevel = 1;
            _ScreenNumbers = new List<uint> { 0 };
            _ExtendRange = false;
            _StartDelay = 2500;
			//_MinGrayLevel = 0;
			//_MaxGrayLevel = 255;
			_SpanScreens = false;
			_DelayBetweenLevels=100;
			_WriteTimings = true;
			_EmailWhenComplete =false;
			_EmailTo = "";
			_SMTPServer = "auth-smtp.bham.ac.uk";
            _SMTPPort = 965;
            _SMTPUser = "";
            _SMTPPassword = "";
			_SpanScreens = false;
			_SpanSide = SpanSideEnum.Full;
			_DeferCalc = false;
			_TargetDevice=TargetDeviceEnum.Not_Set; // must explicitly set target device first
			_RandomizeRepeats=false;

        }

		public static bool Save(string filename, CalibConfig config)
		{
			Yao.XmlSerializer serializer = new Yao.XmlSerializer();
            serializer.SerializationIgnoredAttributeType = typeof(XmlIgnoreAttribute);
			serializer.Serialize(config, filename);
			return true;
				
		}

		public static bool Load(string filename,ref CalibConfig config)
		{
			Yao.XmlDeserializer deserializer = new Yao.XmlDeserializer();
			config = (CalibConfig)deserializer.Deserialize(filename);
			config._DeferCalc = false;
			config.CalcTotalReadings();
			return true;
		}



		
/*
		[Category("Configuration"), Description("No of gray levels"), TypeConverter(typeof(LevelConverter))]
		public string NoOfGrayLevels
		{
			get
			{
				return _GrayLevels.ToString();
			}
			set
			{
				_GrayLevels = uint.Parse(value);

			}
		}
*/

  /*[Category("Configuration"), Description("Maximum gray level")]
		public uint MaxGrayLevel
		{
			get
			{
				return _MaxGrayLevel;
			}
			set
			{
				if (value <= 255)
					_MaxGrayLevel = value;
				else
					throw new ArgumentException("Maximum gray level is 255");

			}
		}*/



		/*[Category("Configuration"), Description("Start delay before first reading in ms")]
		public uint MinGrayLevel
		{
			get
			{
				return _MinGrayLevel;
			}
			set
			{
				if (value <= 255)
					_MinGrayLevel = value;
				else
					throw new ArgumentException("Maximum gray level is 255");
			}
		}*/



        [Category("Configuration"),Description("Start delay before first reading in ms")]
        public uint StartDelay
        {
            get
            {
                return _StartDelay;
            }
            set
            {
                _StartDelay = value;
                NotifyPropertyChanged("StartDelay");
            }
        }

        [Category("Information"),ReadOnly(true),Description("Extend range by including neighbouring bits to the current grey level")]
        public bool ExtendRange
        {
            get
            {
                return _ExtendRange;
            }
            set
            {
                _ExtendRange = value;
                NotifyPropertyChanged("ExtendRange");
                CalcTotalReadings();
            }
        }

        [Category("Information"),Description("Screen number to calibrate"),ReadOnly(true)]
        [XmlIgnore][Browsable(false)]
        public List<uint> ScreenNumbers
        {
            get
            {
                return _ScreenNumbers;
            }
            set
            {
                _ScreenNumbers = value;
                NotifyPropertyChanged("ScreenNumbers");
            }
        }

        [Browsable(false)]
        public CalibConfig Configuration
        {
            get
            {
                return (CalibConfig)this.MemberwiseClone();
            }
        }


        [Category("Configuration"),Description("No. of repeat readings per gray level")]
        public uint ReadingsPerLevel
        {
            get
            {
                return _ReadingsPerLevel;
            }
            set
            {
                _ReadingsPerLevel = value;
                NotifyPropertyChanged("ReadingsPerLevel");
				CalcTotalReadings();
            }
        }

        /// <summary>
        /// file to output the results to
        /// </summary>

        [EditorAttribute(typeof(FileNameEditorEx), typeof(System.Drawing.Design.UITypeEditor)),Category("Configuration"),Description("File to write the output to - path must exist")]
        [XmlIgnore] // user must set output file explicitly before running calibration to prevent overwritten calibration files
		public string OutputFile
        {
            set
            {
                _OutputFile = value;
                NotifyPropertyChanged("OutputFile");

            }

            get
            {
                return Path.GetFileName(_OutputFile);
            }
        }

        public string GetOutputFile()
        {
            return _OutputFile;
        }

        [Category("Information"),Description("Width of currently selected screen in pixels"),ReadOnly(true)]
        [XmlIgnore]
        public uint DisplayWidth
        {
            get
            {
                return _DisplayWidth;
            }
			set
			{
				_DisplayWidth = value;
                NotifyPropertyChanged("DisplayWidth");
			}
        }

        /// <summary>
        /// Screen Height
        /// </summary>
		[Category("Information"),Description("Height of currently selected screen in pixels"),ReadOnly(true)]
        [XmlIgnore]
        public uint DisplayHeight
        {
            get
            {
                return _DisplayHeight;
            }
			set
			{
				_DisplayHeight = value;
                NotifyPropertyChanged("DisplayHeight");
			}
        }

        /// <summary>
        /// Patch width
        /// </summary>
        [Category("Configuration"),Description("Width of patch in pixels")]
		public uint PatchWidth
        {
            get
            {
                return _PatchWidth;
            }
            set
            {
                _PatchWidth = value;
                NotifyPropertyChanged("PatchWidth");
            }
        }

        /// <summary>
        /// Patch Height
        /// </summary>
		[Category("Configuration"),Description("Height of patch in pixels")]
        public uint PatchHeight
        {
            get
            {
                return _PatchHeight;
            }
            set
            {
                _PatchHeight = value;
                NotifyPropertyChanged("PatchHeight");
            }

        }

        /// <summary>
        /// Total no. of readings to take
        /// </summary>
        [Category("Information"),Description("Total no. of readings to take - including repeat readings and readings from Bit stealing")]
		public uint TotalReadings
        {
            get
            {
                return _TotalReadings;
            }
        }


        /// <summary>
        /// No. of readings already made
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public uint TotalReadingsDone
        {
            get
            {
                return _TotalReadingsDone;
            }
            set
            {
                _TotalReadingsDone = value;
                NotifyPropertyChanged("TotalReadingsDone");
            }

        }

        [Browsable(false)]
        [XmlIgnore]
        public uint ReadingsDonePerLevel
        {
            get
            {
                return _ReadingsDonePerLevel;
            }
            set
            {
                _ReadingsDonePerLevel = value;
                NotifyPropertyChanged("ReadingsDonePerLevel");
            }
        }

        /// <summary>
        /// No. of readings still to make
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public uint ReadingsToGo
        {
            get
            {
                return _TotalReadings - _TotalReadingsDone;
            }
        }

		[Category("Configuration"),Description("Step size between successive grey levels"),TypeConverter(typeof(LevelConverter))]
        public uint GrayLevelStepSize
        {
            get
            {
                return _GrayLevelStepSize;
            }
            set
            {
                _GrayLevelStepSize = value;
				CalcTotalReadings();

				NotifyPropertyChanged("TotalReadings");

            }
        }


    }


    internal class FileNameEditorEx : FileNameEditor
    {

        protected override void InitializeDialog(System.Windows.Forms.OpenFileDialog openFileDialog)
        {
            openFileDialog.CheckPathExists = true;
            openFileDialog.CheckFileExists = false;
            openFileDialog.Title = "Select results output file";
			openFileDialog.Filter = "Gamma calibration luminance results files (*.lum)|*.lum";
            openFileDialog.DefaultExt = "lum";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ValidateNames = true;
        }
    }

	internal class LevelConverter : UInt16Converter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			//true means show a combobox

			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			//true will limit to list. false will show the list, 

			//but allow free-form entry

			return true;
		}

		public override System.ComponentModel.TypeConverter.StandardValuesCollection
			   GetStandardValues(ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(CalibConfig._GrayLevelSteps);
		}
	}
	
	internal class FlagsEnumConverter : EnumConverter
	{
		/// <summary>
		/// This class represents an enumeration field in the property grid.
		/// </summary>
		protected class EnumFieldDescriptor : SimplePropertyDescriptor
		{
			#region Fields
			/// <summary>
			/// Stores the context which the enumeration field descriptor was created in.
			/// </summary>
			ITypeDescriptorContext fContext;
			#endregion

			#region Methods
			/// <summary>
			/// Creates an instance of the enumeration field descriptor class.
			/// </summary>
			/// <param name="componentType">The type of the enumeration.</param>
			/// <param name="name">The name of the enumeration field.</param>
			/// <param name="context">The current context.</param>
			public EnumFieldDescriptor(Type componentType, string name, ITypeDescriptorContext context)
				: base(componentType, name, typeof(bool))
			{
				fContext = context;
			}

			/// <summary>
			/// Retrieves the value of the enumeration field.
			/// </summary>
			/// <param name="component">
			/// The instance of the enumeration type which to retrieve the field value for.
			/// </param>
			/// <returns>
			/// True if the enumeration field is included to the enumeration; 
			/// otherwise, False.
			/// </returns>
			public override object GetValue(object component)
			{
				return ((int)component & (int)Enum.Parse(ComponentType, Name)) != 0;
			}

			/// <summary>
			/// Sets the value of the enumeration field.
			/// </summary>
			/// <param name="component">
			/// The instance of the enumeration type which to set the field value to.
			/// </param>
			/// <param name="value">
			/// True if the enumeration field should included to the enumeration; 
			/// otherwise, False.
			/// </param>
			public override void SetValue(object component, object value)
			{
				bool myValue = (bool)value;
				int myNewValue;
				if (myValue)
					myNewValue = ((int)component) | (int)Enum.Parse(ComponentType, Name);
				else
					myNewValue = ((int)component) & ~(int)Enum.Parse(ComponentType, Name);

				FieldInfo myField = component.GetType().GetField("value__", BindingFlags.Instance | BindingFlags.Public);
				myField.SetValue(component, myNewValue);
				fContext.PropertyDescriptor.SetValue(fContext.Instance, component);
			}

			/// <summary>
			/// Retrieves a value indicating whether the enumeration 
			/// field is set to a non-default value.
			/// </summary>
			public override bool ShouldSerializeValue(object component)
			{
				return (bool)GetValue(component) != GetDefaultValue();
			}

			/// <summary>
			/// Resets the enumeration field to its default value.
			/// </summary>
			public override void ResetValue(object component)
			{
				SetValue(component, GetDefaultValue());
			}

			/// <summary>
			/// Retrieves a value indicating whether the enumeration 
			/// field can be reset to the default value.
			/// </summary>
			public override bool CanResetValue(object component)
			{
				return ShouldSerializeValue(component);
			}

			/// <summary>
			/// Retrieves the enumerations field’s default value.
			/// </summary>
			private bool GetDefaultValue()
			{
				object myDefaultValue = null;
				string myPropertyName = fContext.PropertyDescriptor.Name;
				Type myComponentType = fContext.PropertyDescriptor.ComponentType;

				// Get DefaultValueAttribute
				DefaultValueAttribute myDefaultValueAttribute = (DefaultValueAttribute)Attribute.GetCustomAttribute(
					myComponentType.GetProperty(myPropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic),
					typeof(DefaultValueAttribute));
				if (myDefaultValueAttribute != null)
					myDefaultValue = myDefaultValueAttribute.Value;

				if (myDefaultValue != null)
					return ((int)myDefaultValue & (int)Enum.Parse(ComponentType, Name)) != 0;
				return false;
			}
			#endregion

			#region Properties
			public override AttributeCollection Attributes
			{
				get
				{
					return new AttributeCollection(new Attribute[] { RefreshPropertiesAttribute.Repaint });
				}
			}
			#endregion
		}

		#region Methods
		/// <summary>
		/// Creates an instance of the FlagsEnumConverter class.
		/// </summary>
		/// <param name="type">The type of the enumeration.</param>
		public FlagsEnumConverter(Type type) : base(type) { }

		/// <summary>
		/// Retrieves the property descriptors for the enumeration fields. 
		/// These property descriptors will be used by the property grid 
		/// to show separate enumeration fields.
		/// </summary>
		/// <param name="context">The current context.</param>
		/// <param name="value">A value of an enumeration type.</param>
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			if (context != null)
			{
				Type myType = value.GetType();
				string[] myNames = Enum.GetNames(myType);
				Array myValues = Enum.GetValues(myType);
				if (myNames != null)
				{
					PropertyDescriptorCollection myCollection = new PropertyDescriptorCollection(null);
					for (int i = 0; i < myNames.Length; i++)
					{
						if ((int)myValues.GetValue(i) != 0 && myNames[i] != "All")
							myCollection.Add(new EnumFieldDescriptor(myType, myNames[i], context));
					}
					return myCollection;
				}
			}
			return base.GetProperties(context, value, attributes);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			if (context != null)
			{
				return true;
			}
			return base.GetPropertiesSupported(context);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return false;
		}
		#endregion

	}
}

