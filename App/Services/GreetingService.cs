using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace App.Services {
	public class GreetingService : IGreetingService {
		private readonly ILogger<GreetingService> log;
		private readonly IConfiguration config;

		public GreetingService(ILogger<GreetingService> log, IConfiguration config) {
			this.log = log ?? throw new ArgumentNullException(nameof(log));
			this.config = config ?? throw new ArgumentNullException(nameof(config));
		}

		public void Run() {
			int loopTimes = this.config.GetValue<int>("LoopTimes");
			for (int i = 0; i < loopTimes; i++) {
				this.log.LogInformation("Run number {iteration}", i);
			}
		}
	}
}
