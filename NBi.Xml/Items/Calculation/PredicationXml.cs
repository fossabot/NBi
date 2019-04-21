﻿using NBi.Core.Calculation;
using NBi.Core.Calculation.Predicate;
using NBi.Core.ResultSet;
using NBi.Xml.Constraints.Comparer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation
{
    public class PredicationXml : IPredicateInfo, ISecondOperandPredicateInfo, ICultureSensitivePredicateInfo, ICaseSensitivePredicateInfo 
    {
        public PredicationXml()
        {
            ColumnType = ColumnType.Numeric;
        }

        [XmlIgnore()]
        [XmlAttribute("column-index")]
        [Obsolete("Deprecated. Use operand in place of column-index")]
        public int ColumnIndex
        {
            get => throw new InvalidOperationException();
            set => Operand = new ColumnIdentifierFactory().Instantiate($"#{value}");
        }

        [XmlIgnore]
        public bool Not
        {
            get => Predicate.Not; 
            set => Predicate.Not = value;
        }

        [XmlAttribute("operand")]
        public string OperandSerialized
        {
            get => Operand?.Label;
            set { Operand = new ColumnIdentifierFactory().Instantiate(value); }
        }

        [XmlIgnore()]
        public IColumnIdentifier Operand { get; set; }

        [Obsolete("Deprecated. Use operand in place of name")]
        [XmlIgnore()]
        public string Name { get => Operand.Label; set => Operand=new ColumnIdentifierFactory().Instantiate(value); }

        [DefaultValue(ColumnType.Numeric)]
        [XmlAttribute("type")]
        public ColumnType ColumnType { get; set; }

        [XmlElement(Type = typeof(LessThanXml), ElementName ="less-than")]
        [XmlElement(Type = typeof(MoreThanXml), ElementName = "more-than")]
        [XmlElement(Type = typeof(EqualXml), ElementName = "equal")]
        [XmlElement(Type = typeof(NullXml), ElementName = "null")]
        [XmlElement(Type = typeof(EmptyXml), ElementName = "empty")]
        [XmlElement(Type = typeof(LowerCaseXml), ElementName = "lower-case")]
        [XmlElement(Type = typeof(UpperCaseXml), ElementName = "upper-case")]
        [XmlElement(Type = typeof(StartsWithXml), ElementName = "starts-with")]
        [XmlElement(Type = typeof(EndsWithXml), ElementName = "ends-with")]
        [XmlElement(Type = typeof(ContainsXml), ElementName = "contains")]
        [XmlElement(Type = typeof(MatchesRegexXml), ElementName = "matches-regex")]
        [XmlElement(Type = typeof(MatchesNumericXml), ElementName = "matches-numeric")]
        [XmlElement(Type = typeof(MatchesDateXml), ElementName = "matches-date")]
        [XmlElement(Type = typeof(MatchesTimeXml), ElementName = "matches-time")]
        [XmlElement(Type = typeof(WithinRangeXml), ElementName = "within-range")]
        [XmlElement(Type = typeof(AnyOfXml), ElementName = "any-of")]
        [XmlElement(Type = typeof(IntegerXml), ElementName = "integer")]
        [XmlElement(Type = typeof(ModuloXml), ElementName = "modulo")]
        [XmlElement(Type = typeof(OnTheDayXml), ElementName = "on-the-day")]
        [XmlElement(Type = typeof(OnTheHourXml), ElementName = "on-the-hour")]
        [XmlElement(Type = typeof(OnTheMinuteXml), ElementName = "on-the-minute")]
        [XmlElement(Type = typeof(TrueXml), ElementName = "true")]
        [XmlElement(Type = typeof(FalseXml), ElementName = "false")]
        public PredicateXml Predicate { get; set; }

        private object reference;
        [XmlIgnore]
        public object Reference
        {
            get { return reference ?? Predicate.Value ?? Predicate.Values as object; }
            set { reference = value; }
        }

        [XmlIgnore]
        public object SecondOperand
        {
            get { return (Predicate as ITwoOperandsXml)?.SecondOperand; }
        }

        [XmlIgnore]
        public StringComparison StringComparison
        {
            get
            {
                if (Predicate is CaseSensitiveTextPredicateXml)
                    return ((CaseSensitiveTextPredicateXml)Predicate).IgnoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
                else
                    return StringComparison.InvariantCulture;
            }
        }

        [XmlIgnore]
        public string Culture
        {
            get
            {
                if (Predicate is CultureSensitiveTextPredicateXml)
                    return ((CultureSensitiveTextPredicateXml)Predicate).Culture;
                else
                    return CultureInfo.InvariantCulture.Name;
            }
        }

        [XmlIgnore]
        public ComparerType ComparerType { get => Predicate.ComparerType; }
    }
}
