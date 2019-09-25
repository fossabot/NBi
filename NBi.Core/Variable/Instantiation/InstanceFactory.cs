﻿using NBi.Core.Sequence.Resolver;
using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable.Instantiation
{
    public class InstanceFactory
    {
        public IEnumerable<Instance> Instantiate(IInstanceArgs args)
        {
            switch (args)
            {
                case DefaultInstanceArgs _: return new[] { Instance.Default };
                case DerivatedVariableInstanceArgs s: return Instantiate(s.Name, s.Resolver, s.Derivations, args.Categories, args.Traits);
                case SingleVariableInstanceArgs s: return Instantiate(s.Name, s.Resolver, args.Categories, args.Traits);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<Instance> Instantiate(string variableName, ISequenceResolver resolver, IEnumerable<string> categories, IDictionary<string, string> traits)
        {
            foreach (var obj in resolver.Execute())
            {
                var instanceVariable = new InstanceVariable(obj);
                yield return new Instance(
                    new Dictionary<string, ITestVariable>() { { variableName, instanceVariable } },
                    categories,
                    traits
                    );
            }
        }

        private IEnumerable<Instance> Instantiate(string variableName, ISequenceResolver resolver, IDictionary<string, DerivationArgs> derivations, IEnumerable<string> categories, IDictionary<string, string> traits)
        {
            foreach (var obj in resolver.Execute())
            {
                var dico = new Dictionary<string, ITestVariable>() { { variableName, new InstanceVariable(obj) } };
                foreach (var derivation in derivations)
                    dico.Add(derivation.Key, new InstanceVariable(derivation.Value.Transformer.Execute(dico[derivation.Value.Source].GetValue())));
                yield return new Instance(
                    dico,
                    categories,
                    traits
                    );
            }
        }
    }
}
