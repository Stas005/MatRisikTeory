var jobs = new[]
{
    new[] { (Salary: 2000.0, Prob: 1.0) },
    new[] { (Salary: 3000.0, Prob: 0.5), (Salary: 1000.0, Prob: 0.5) },
    new[] { (Salary: 4000.0, Prob: 0.5), (Salary: 0.0, Prob: 0.5) }
};

var jobsStatistics = jobs.Select((job, index) =>
{
    var expectedWin = job.Sum(j => j.Salary * j.Prob);

    var expectedBenefit = job.Sum(j => j.Prob * (0.01 * j.Salary * j.Salary));

    var dEquivalent = Math.Sqrt(100 * expectedBenefit);

    var riskPremium = expectedBenefit - dEquivalent;

    return (JobIndex: index + 1, EW: expectedWin, EB: expectedBenefit, DE: dEquivalent, RP: riskPremium);
}).ToList();

var bestJob = jobsStatistics.OrderByDescending(j => j.EB).First();

Console.WriteLine($"The best choice is job N{bestJob.JobIndex} with expected win:{bestJob.EW}, expected benefit:{bestJob.EB}, d-equivalent:{bestJob.DE}, risk premium:{bestJob.RP}");