import { RouteReuseStrategy, ActivatedRouteSnapshot, DetachedRouteHandle, RouterModule, Routes, UrlSegment } from '@angular/router';
export class RouteReuseService implements RouteReuseStrategy {
    private storedRoutes = new Map<string, DetachedRouteHandle>();
    shouldDetach(route: ActivatedRouteSnapshot): boolean {
        if (!route.routeConfig || route.routeConfig.loadChildren) {
            return false;
        }
        let shouldReuse = false;
        if (route.routeConfig?.data) {
            route.routeConfig.data.reuse ? shouldReuse = true : shouldReuse = false;
        }
        return shouldReuse;
    }
    store(route: ActivatedRouteSnapshot, handler: DetachedRouteHandle): void {
        let url = this.getUrl(route);
        if (route.routeConfig?.data?.reuse && url && url.length > 0) {
            this.storedRoutes.set(url, handler);
        }

    }
    shouldAttach(route: ActivatedRouteSnapshot): boolean {
        let url = this.getUrl(route);
        let shouldAttach = false;
        if (route.routeConfig?.data?.reuse && url && url.length>0 && !!this.storedRoutes.get(url)) {
            shouldAttach = true;
        }
        return shouldAttach;
    }
    retrieve(route: ActivatedRouteSnapshot): DetachedRouteHandle | null {
        if (!route.routeConfig || route.routeConfig.loadChildren) {
            return null;
        };
        let url = this.getUrl(route);
        let handler = null;
        if (url && url.length > 0) {
            handler = this.storedRoutes.get(url) ?? null;
        } 
        return handler;
    }
    shouldReuseRoute(future: ActivatedRouteSnapshot, current: ActivatedRouteSnapshot): boolean {
        let reUseUrl = false;
        if (future.routeConfig) {
            if (future.routeConfig.data) {
                reUseUrl = future.routeConfig.data.reuse;
            }
        }
        const defaultReuse = (future.routeConfig === current.routeConfig);
        return reUseUrl;
    }
    getUrl(route: ActivatedRouteSnapshot): string | null {
        if (route.routeConfig) {
            const url = route.routeConfig.path;
            return url ?? null;
        }
        return null;
    }
}