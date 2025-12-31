//@IntentMerge()

// interface for all environment configuration
//@IntentMerge()
export interface AppEnvironment {
  dotNetBackEndServiceServiceConfig: DotNetBackEndServiceServiceConfig;
}

// base config for http service proxies
export interface HttpConfig {
  baseUrl: string;
  retries?: number;
  timeoutMs?: number;
}

// interface allows for overrides per service
export interface ServiceOverride {
  baseUrl?: string;
  retries?: number;
  timeoutMs?: number;
}

// optional overrides for DotNetBackEndServiceService
export interface DotNetBackEndServiceServiceServicesConfig {
  productsService?: ServiceOverride;
}

// specific configuration for DotNetBackEndServiceService
export interface DotNetBackEndServiceServiceConfig extends HttpConfig {
  services?: DotNetBackEndServiceServiceServicesConfig;
}
