package com.AngularTest.JavaApplication.application.rest;

public class JsonResponse<T> {
    public JsonResponse(T value) {
        this.value = value;
    }

    private T value;
    public T getValue() {
        return this.value;
    }
    public void setValue(T value) {
        this.value = value;
    }
}