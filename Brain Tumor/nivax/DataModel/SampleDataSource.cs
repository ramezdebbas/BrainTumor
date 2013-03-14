using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Causes & Symptoms",
                    "Causes & Symptoms",
                    "Assets/Images/10.jpg",
                    "A primary brain tumor is a group (mass) of abnormal cells that start in the brain. This article focuses on primary brain tumors in adults.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Causes",
                    "Primary brain tumors include any tumor that starts in the brain. Primary brain tumors can arise from the brain cells, the membranes around the brain (meninges), nerves, or glands.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nPrimary brain tumors include any tumor that starts in the brain. Primary brain tumors can arise from the brain cells, the membranes around the brain (meninges), nerves, or glands.\n\nTumors can directly destroy brain cells. They can also damage cells by producing inflammation, placing pressure on other parts of the brain, and increasing pressure within the skull.\n\nThe cause of primary brain tumors is unknown. There are many possible risk factors that could play a role.\n\nRadiation therapy to the brain, used to treat brain cancers, increases the risk for brain tumors up to 20 or 30 years afterwards.Exposure to radiation at work or to power lines, as well as head injuries, smoking, and hormone replacement therapy have NOT yet been shown to be factors.The risk of using cell phones is hotly debated. However, most recent studies have found that cell phones, cordless phones, and wireless devices are safe and do not increase the risk.\n\nSome inherited conditions increase the risk of brain tumors, including neurofibromatosis, Von Hippel-Lindau syndrome, Li-Fraumeni syndrome, and Turcot syndrome.\n\nSPECIFIC TUMOR TYPES\n\nBrain tumors are classified depending on the exact site of the tumor, the type of tissue involved, whether they are noncancerous (benign) or cancerous (malignant), and other factors. Sometimes, tumors that start out being less invasive can become more invasive.\n\nTumors may occur at any age, but many types of tumors are most common in a certain age group. In adults, gliomas and meningiomas are most common.Gliomas come from glial cells such as astrocytes, oligodendrocytes, and ependymal cells. The gliomas are divided into three types:\n\nAstrocytic tumors include astrocytomas (less malignant), anaplastic astrocytomas, and glioblastomas (most malignant).Oligodendroglial tumors also can vary from less malignant to very malignant. Some primary brain tumors are made up of both astrocytic and oligodendrocytic tumors. These are called mixed gliomas.Glioblastomas are the most aggressive type of primary brain tumor.\n\nMeningiomas are another type of brain tumor. These tumors:Occur most commonly between the ages of 40 - 70Are much more common in womenAre usually (90% of the time) benign, but still may cause devastating complications and death due to their size or location. Some are cancerous and aggressive.Other primary brain tumors in adults are rare. These include:Ependymomas\nCraniopharyngiomas\nPituitary tumors\nPrimary lymphoma of the brain\nPineal gland tumors\nPrimary germ cell tumors of the brain",
                    group1) { CreatedOn = "Group", CreatedTxt = "Causes & Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Causes", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Brain Tumor" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Symptoms",
                     "A doctor can often identify signs and symptoms that are specific to the tumor location. Some tumors may not cause symptoms until they are very large. Then they can lead to a rapid decline in the person's health. Other tumors have symptoms that develop slowly.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nA doctor can often identify signs and symptoms that are specific to the tumor location. Some tumors may not cause symptoms until they are very large. Then they can lead to a rapid decline in the person's health. Other tumors have symptoms that develop slowly.\n\nThe specific symptoms depend on the tumor's size, location, how far it has spread, and related swelling. The most common symptoms are:\nHeadaches\nSeizures (especially in older adults)\nWeakness in one part of the body\nChanges in the person's mental functions\nHeadaches caused by brain tumors may:\nBe worse when the person wakes up in the morning, and clear up in a few hours\nOccur during sleep\nBe accompanied by vomiting, confusion, double vision, weakness, or numbness\nGet worse with coughing or exercise, or with a change in body position\nOther symptoms may include:\nChange in alertness (including sleepiness, unconsciousness, and coma)\nChanges in hearing\nChanges in taste or smell\nChanges that affect touch and the ability to feel pain, pressure, different temperatures, or other stimuli\nClumsiness\nConfusion or memory loss\nDifficulty swallowing\nDifficulty writing or reading\nDizziness or abnormal sensation of movement (vertigo)\nEye abnormalities\nEyelid drooping\nPupils different sizes\nUncontrollable movements\nHand tremor\nLack of control over the bladder or bowels\nLoss of balance\nLoss of coordination\nMuscle weakness in the face, arm, or leg (usually on just one side)\nNumbness or tingling on one side of the body\nPersonality, mood, behavioral, or emotional changes\nProblems with eyesight, including decreased vision, double vision, or total loss of vision\nTrouble speaking or understanding others who are speaking\nTrouble walking\nOther symptoms that may occur with a pituitary tumor:\nAbnormal nipple discharge\nAbsent menstruation (periods)\nBreast development in men\nEnlarged hands, feet\nExcessive body hair\nFacial changes\nLow blood pressure\nObesity\nSensitivity to heat or cold",
                     group1) { CreatedOn = "Group", CreatedTxt = "Causes & Symptoms", CreatedOnTwo = "Item", CreatedTxtTwo = "Symptoms", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Brain Tumor" });
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                   "Exams & Tests",
                   "Exams & Tests",
                   "Assets/Images/20.jpg",
                   "Most brain tumors increase pressure within the skull and compress brain tissue because of their size and weight. The following tests may confirm the presence of a brain tumor and identify its location");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Cranial CT Scan",
                    "A cranial computed tomography (CT) scan uses many x-rays to create pictures of the head, including the skull, brain, eye sockets, and sinuses.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nYou will be asked to lie on a narrow table that slides into the center of the CT scanner.Once you are inside the scanner, the machine's x-ray beam rotates around you. (Modern spiral scanners can perform the exam without stopping.)\n\nA computer creates separate images of the body area, called slices. These images can be stored, viewed on a monitor, or printed on film. Three-dimensional models of the head area can be created by stacking the slices together.\n\nYou must be still during the exam, because movement causes blurred images. You may be told to hold your breath for short periods of time.\n\nGenerally, complete scans take only a few minutes. The newest scanners can image your entire body, head to toe, in less than 30 seconds.",
                    group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Cranial CT Scan", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/21.jpg")), CurrentStatus = "Brain Tumor" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                     "EEG",
                     "An electroencephalogram (EEG) is a test to measure the electrical activity of the brain.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nBrain cells talk to each other by producing tiny electrical signals, called impulses.An EEG helps measure this activity. The test is done by a EEG specialist in your doctor's office or at a hospital or laboratory.\n\nYou will be asked to lie on your back on a bed or in a reclining chair.Flat metal disks called electrodes are placed all over your scalp. The disks are held in place with a sticky paste. The electrodes are connected by wires to a speaker and recording machine.\n\nThe recording machine changes the electrical signals into patterns that can be seen on a computer. It looks like a bunch of wavy lines.\n\nYou will need to lie still during the test with your eyes closed because movement can change the results. But, you may be asked to do certain things during the test, such as breathe fast and deeply for several minutes or look at a bright flashing light.",
                     group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "EEG", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/22.jpg")), CurrentStatus = "Brain Tumor" });
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                      "Biopsy",
                      "A biopsy is the removal of a small piece of tissue for laboratory examination.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nThere are several different types of biopsies.\nA needle (percutaneous) biopsy removes tissue using a hollow tube called a syringe. A needle is passed several times through the tissue being examined. The surgeon uses the needle to remove the tissue sample. Needle biopsies are often done using x-rays (usually CT scan or ultrasound), which guide the surgeon to the right area.\n\nAn open biopsy is a surgical procedure that uses local or general anesthesia. This means you are relaxed (sedated) or asleep and pain-free during the procedure. The procedure is done in a hospital operating room. The surgeon makes a cut into the affected area, and the tissue is removed.\n\nClosed biopsy uses a much smaller surgical cut than open biopsy. A small cut is made so that a camera-like instrument can be inserted. This instrument helps guide the surgeon to the right place to take the sample.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Biopsy", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/23.jpg")), CurrentStatus = "Brain Tumor" });
			group2.Items.Add(new SampleDataItem("Group-2-Item-4",
                      "Head MRI",
                      "A head MRI (magnetic resonance imaging) scan of the head is a imaging test that uses powerful magnets and radio waves to create pictures of the brain and surrounding nerve tissues.",
                      "Assets/DarkGray.png",
                      "",
                      "Details:\n\nYou may be asked to wear a hospital gown or clothing without metal fasteners (such as sweatpants and a t-shirt). Certain types of metal can cause blurry images.\n\nYou will lie on a narrow table, which slides into a large tunnel-shaped scanner.\n\nSome exams require a special dye (contrast). The dye is usually given before the test through a vein (IV) in your hand or forearm. The dye helps the radiologist see certain areas more clearly.\n\nDuring the MRI, the person who operates the machine will watch you from another room. The test most often lasts 30 - 60 minutes, but it may take longer.",
                      group2) { CreatedOn = "Group", CreatedTxt = "Exams & Tests", CreatedOnTwo = "Item", CreatedTxtTwo = "Head MRI", bgColour = "#20B2AA", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/24.jpg")), CurrentStatus = "Brain Tumor" });	
				this.AllGroups.Add(group2);
			
            var group3 = new SampleDataGroup("Group-3",
                   "Treatment",
                   "Treatment",
                   "Assets/Images/30.jpg",
                   "Treatment can involve surgery, radiation therapy, and chemotherapy. Brain tumors are best treated by a team involving a neurosurgeon, radiation oncologist, oncologist, or neuro-oncologist, and other health care providers, such as neurologists and social workers.");
            group3.Items.Add(new SampleDataItem("Group-3-Item-1",
                    "Research Methodology",
                    "Treatment can involve surgery, radiation therapy, and chemotherapy. Brain tumors are best treated by a team involving a neurosurgeon, radiation oncologist, oncologist, or neuro-oncologist, and other health care providers, such as neurologists and social workers.",
                    "Assets/DarkGray.png",
                    "",
                    "Details:\n\nTreatment can involve surgery, radiation therapy, and chemotherapy. Brain tumors are best treated by a team involving a neurosurgeon, radiation oncologist, oncologist, or neuro-oncologist, and other health care providers, such as neurologists and social workers.\n\nEarly treatment often improves the chance of a good outcome. Treatment, however, depends on the size and type of tumor and the general health of the patient. The goals of treatment may be to cure the tumor, relieve symptoms, and improve brain function or the person's comfort.\n\nSurgery is often necessary for most primary brain tumors. Some tumors may be completely removed. Those that are deep inside the brain or that enter brain tissue may be debulked instead of entirely removed. Debulking is a procedure to reduce the tumor's size.\n\nTumors can be difficult to remove completely by surgery alone, because the tumor invades surrounding brain tissue much like roots from a plant spread through soil. When the tumor cannot be removed, surgery may still help reduce pressure and relieve symptoms.",
                    group3) { CreatedOn = "Group", CreatedTxt = "Treatment", CreatedOnTwo = "Item", CreatedTxtTwo = "Research Methodology", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/31.jpg")), CurrentStatus = "Brain Tumor" });
            group3.Items.Add(new SampleDataItem("Group-3-Item-2",
                     "Radiation",
                     "Radiation therapy is used for certain tumors.Chemotherapy may be used along with surgery or radiation treatment.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nRadiation therapy is used for certain tumors.\nChemotherapy may be used along with surgery or radiation treatment.Other medications used to treat primary brain tumors in children may include:\nCorticosteroids, such as dexamethasone to reduce brain swelling Osmotic diuretics, such as urea or mannitol to reduce brain swelling and pressure Anticonvulsants, such as evetiracetam (Keppra) to reduce seizures\nPain medications\nAntacids or histamine blockers to control stress ulcers\nComfort measures, safety measures, physical therapy, and occupational therapy may be needed to improve quality of life. The patient may need counseling, support groups, and similar measures to help cope with the disorder.\n\nPatients may also consider enrolling in a clinical trial after talking with their treatment team.\n\nLegal advice may be helpful in creating advanced directives such as a power of attorney.",
                     group3) { CreatedOn = "Group", CreatedTxt = "Treatment", CreatedOnTwo = "Item", CreatedTxtTwo = "Radiation", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/32.jpg")), CurrentStatus = "Brain Tumor" });
            this.AllGroups.Add(group3);
			
			
         
        }
    }
}
