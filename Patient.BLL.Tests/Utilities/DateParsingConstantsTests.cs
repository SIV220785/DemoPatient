using System.Globalization;
using Patient.BLL.Constans;
using Patient.BLL.Utilities;
using Patient.DAL.Entities;

namespace Patient.BLL.Tests.Utilities
{
    [TestFixture]
    public class DateParsingConstantsTests
    {
        private List<PatientProfile> _patients;

        [SetUp]
        public void Setup()
        {
            _patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 13, 12, 0, 0) }, // ne
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 0, 0, 0) },  // eq, lt
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 8, 59, 0) }, // eq,
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 10, 0, 0) }, // eq,
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 9, 59, 0) }, // eq,
                new PatientProfile { RecordDate = new DateTime(2013, 1, 15, 0, 0, 0) },  // ne,
                
                //new() {RecordDate = new DateTime(2023, 1, 15)},
                //new() {RecordDate = new DateTime(2013, 3, 14)},
                //new() {RecordDate = new DateTime(2013, 2, 1)},
                //new() {RecordDate = new DateTime(2013, 3, 13)},
                //new() {RecordDate = new DateTime(2013, 4, 14)}
            ];
        }

        [TestCase("eq2013-01-14", "eq", "2013-01-14")]
        [TestCase("ne2013-01-14", "ne", "2013-01-14")]
        [TestCase("lt2013-01-14T10:00", "lt", "2013-01-14 10:00")]
        [TestCase("gt2013-01-14T10:00", "gt", "2013-01-14 10:00")]
        [TestCase("ge2013-03-14", "ge", "2013-03-14")]
        [TestCase("le2013-03-14", "le", "2013-03-14")]
        [TestCase("sa2013-03-14", "sa", "2013-03-14")]
        [TestCase("eb2013-03-14", "eb", "2013-03-14")]
        [TestCase("ap2013-03-14", "ap", "2013-03-14")]

        public void ParseParameter_ActualFilterParameter_ReturnsOperatorTypeAndDate(string filterParameter, string expectedOperatorType, string expectedDateString)
        {
            var expectedDate = DateTime.ParseExact(expectedDateString, DateParsingConstants.DateFormats,
                CultureInfo.InvariantCulture, DateTimeStyles.None);

            var (actualOperatorType, actualDate) = DateFilterExpressionBuilder.ParseParameter(filterParameter);

            Assert.That(actualOperatorType, Is.EqualTo(expectedOperatorType));
            Assert.That(expectedDate, Is.EqualTo(actualDate));
        }

        [Test]
        public void BuildExpression_FilterParameterIsEQ_ReturnsActualPatient()
        {
            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 10, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 15, 0, 0, 0) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 10, 0, 0) }
            ];

            var filterParameter = "eq2013-01-14";

            var expression = DateFilterExpressionBuilder.BuildExpression(filterParameter);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }

        [Test]
        public void BuildExpression_FilterParameterIsNE_ReturnsActualPatient()
        {
            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 15, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 10, 0, 0) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 15, 0, 0, 0) }
            ];

            var filterParameter = "ne2013-01-14";

            var expression = DateFilterExpressionBuilder.BuildExpression(filterParameter);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }

        [Test]
        public void BuildExpression_FilterParameterIsGE_ReturnsActualPatient()
        {
            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 20, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 15, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2014, 1, 14, 0, 0, 0) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 15, 0, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2014, 1, 14, 0, 0, 0) }
            ];

            var filterParameter = "ge2013-03-14";

            var expression = DateFilterExpressionBuilder.BuildExpression(filterParameter);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }

        [Test]
        public void BuildExpression_FilterParameterIsLE_ReturnsActualPatient()
        {
            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 20) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 21) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 15) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 20) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 21) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14) }
            ];

            var filterParameter = "le2013-03-14";

            var expression = DateFilterExpressionBuilder.BuildExpression(filterParameter);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }

        [Test]
        public void BuildExpression_FilterParameterIsSA_ReturnsActualPatient()
        {
            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 20) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 21) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 15) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 16) },
                new PatientProfile { RecordDate = new DateTime(2013, 4, 1) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 3, 15) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 16) },
                new PatientProfile { RecordDate = new DateTime(2013, 4, 1) }
            ];

            var filterParameter = "sa2013-03-14";

            var expression = DateFilterExpressionBuilder.BuildExpression(filterParameter);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }

        [Test]
        public void BuildExpression_FilterParameterIsEB_ReturnsActualPatient()
        {
            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 20) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 21) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 13) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 15) },
                new PatientProfile { RecordDate = new DateTime(2013, 4, 1) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 20) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 21) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 13) }
            ];

            var filterParameter = "eb2013-03-14";

            var expression = DateFilterExpressionBuilder.BuildExpression(filterParameter);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }

        [Test]
        public void BuildExpression_FilterParameterIsAP_ReturnsActualPatient()
        {
            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 21) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14, 2, 0 , 0) },
                new PatientProfile { RecordDate = new DateTime(2015, 6, 15) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14) },
                new PatientProfile { RecordDate = new DateTime(2013, 3, 14, 2, 0 , 0) }
            ];

            var filterParameter = "ap2013-03-14";

            var expression = DateFilterExpressionBuilder.BuildExpression(filterParameter);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }

        [Test]
        public void BuildExpressionByPeriod_FilterParameterIsLT_ReturnsActualPatient()
        {
            var startPeriod = new DateTime(2013, 1, 14, 08, 0, 0);

            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 9, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 10, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 13, 12, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 8, 0, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 15, 8, 0, 0) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile {RecordDate = new DateTime(2013, 1, 14, 9, 0, 0)},
                new PatientProfile {RecordDate = new DateTime(2013, 1, 14)},
                new PatientProfile {RecordDate = new DateTime(2013, 1, 14, 8, 0, 0)}
            ];

            var filterParameter = "lt2013-01-14T10:00";

            var expression = DateFilterExpressionBuilder.BuildExpressionByPeriod(filterParameter, startPeriod: startPeriod);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }

        //[TestCase("gt2013-01-14T10:00", 1)]
        [Test]
        public void BuildExpressionByPeriod_FilterParameterIsGT_ReturnsActualPatient()
        {
            var endPeriod = new DateTime(2013, 1, 15, 8, 0, 0);

            List<PatientProfile> patients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 9, 30, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 10, 30, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 11, 45, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 15) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 13, 23, 0, 0) }
            ];

            List<PatientProfile> expectedPatients =
            [
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 10, 30, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 14, 11, 45, 0) },
                new PatientProfile { RecordDate = new DateTime(2013, 1, 15) },
            ];

            var filterParameter = "gt2013-01-14T10:00";

            var expression = DateFilterExpressionBuilder.BuildExpressionByPeriod(filterParameter, endPeriod: endPeriod);

            var actualPatients = patients.AsQueryable().Where(expression).ToList();

            Assert.That(ObjectExtensions.ToJson(actualPatients), Is.EqualTo(ObjectExtensions.ToJson(expectedPatients)));
        }
    }
}
