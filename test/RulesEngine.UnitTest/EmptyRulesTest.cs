﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Newtonsoft.Json;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace RulesEngine.UnitTest
{
    [ExcludeFromCodeCoverage]
    public class EmptyRulesTest
    {
        [Fact]
        private async Task EmptyRules_ReturnsExepectedResults()
        {
            var workflows = GetEmptyWorkflows();
            var reSettings = new ReSettings { };
            RulesEngine rulesEngine = new RulesEngine();

            Func<Task> action = () => {
                new RulesEngine(workflows, reSettings: reSettings);
                return Task.CompletedTask;
            };

            Exception ex = await Assert.ThrowsAsync<Exceptions.RuleValidationException>(action);

            Assert.Contains("Atleast one of Rules or WorkflowRulesToInject must be not empty", ex.Message);
        }
        [Fact]
        private async Task NestedRulesWithEmptyNestedActions_ReturnsExepectedResults()
        {
            var workflows = GetEmptyNestedWorkflows();
            var reSettings = new ReSettings { };
            RulesEngine rulesEngine = new RulesEngine();

            Func<Task> action = () => {
                new RulesEngine(workflows, reSettings: reSettings);
                return Task.CompletedTask;
            };

            Exception ex = await Assert.ThrowsAsync<Exceptions.RuleValidationException>(action);

            Assert.Contains("Atleast one of Rules or WorkflowRulesToInject must be not empty", ex.Message);
        }

        private WorkflowRules[] GetEmptyWorkflows()
        {
            return new[] {
                new WorkflowRules {
                    WorkflowName = "EmptyRulesTest",
                    Rules = new Rule[] {
                    }
                }
            };
        }

        private WorkflowRules[] GetEmptyNestedWorkflows()
        {
            return new[] {
                new WorkflowRules {
                    WorkflowName = "EmptyNestedRulesTest",
                    Rules = new Rule[] {
                        new Rule {
                            RuleName = "AndRuleTrueFalse",
                            Operator = "And",
                            Rules = new Rule[] {
                                new Rule{
                                    RuleName = "trueRule1",
                                    Expression = "input1.TrueValue == true",
                                },
                                new Rule {
                                    RuleName = "falseRule1",
                                    Expression = "input1.TrueValue == false"
                                }

                            }
                        },
                        new Rule {
                            RuleName = "OrRuleTrueFalse",
                            Operator = "Or",
                            Rules = new Rule[] {
                                new Rule{
                                    RuleName = "trueRule2",
                                    Expression = "input1.TrueValue == true",
                                },
                                new Rule {
                                    RuleName = "falseRule2",
                                    Expression = "input1.TrueValue == false"
                                }

                            }
                        },
                        new Rule {
                            RuleName = "AndRuleFalseTrue",
                            Operator = "And",
                            Rules = new Rule[] {
                                new Rule{
                                    RuleName = "trueRule3",
                                    Expression = "input1.TrueValue == false",
                                },
                                new Rule {
                                    RuleName = "falseRule4",
                                    Expression = "input1.TrueValue == true"
                                }

                            }
                        },
                         new Rule {
                            RuleName = "OrRuleFalseTrue",
                            Operator = "Or",
                            Rules = new Rule[] {
                                new Rule{
                                    RuleName = "trueRule3",
                                    Expression = "input1.TrueValue == false",
                                },
                                new Rule {
                                    RuleName = "falseRule4",
                                    Expression = "input1.TrueValue == true"
                                }

                            }
                         }
                    }
                },
                new WorkflowRules {
                    WorkflowName = "EmptyNestedRulesActionsTest",
                    Rules = new Rule[] {
                        new Rule {
                            RuleName = "AndRuleTrueFalse",
                            Operator = "And",
                            Rules = new Rule[] {

                            },
                            Actions =  new RuleActions {
                                        OnFailure = new ActionInfo{
                                            Name = "OutputExpression",
                                            Context = new Dictionary<string, object> {
                                                { "Expression", "input1.TrueValue" }
                                            }
                                        }
                                    }
                        }
                    }
                }

            };
        }
    }
}
